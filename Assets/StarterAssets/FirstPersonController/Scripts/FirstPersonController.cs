using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.1f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
        public LayerMask GroundLayers;

        [Header("Footsteps")]
        public AudioClip[] footstepClips;
        public float footstepInterval = 0.5f;
        public float sprintFootstepInterval = 0.3f;

        [Header("Footstep Audio Pool")]
        public int footstepSourceCount = 4;

        [Header("Footstep Reverb")]
        public bool useReverb = true;
        public AudioReverbPreset reverbPreset = AudioReverbPreset.Cave;

        public bool canMove = true;

        private float _speed;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private AudioSource _audioSource;
        private AudioSource[] _footstepSources;
        private AudioReverbFilter[] _footstepReverbs;

        private int _currentFootstepSource = 0;
        private float _footstepTimer = 0f;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _audioSource = GetComponent<AudioSource>();

#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies.");
#endif

            CreateFootstepAudioPool();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void OnEnable()
        {
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _verticalVelocity = 0f;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            HandleFootsteps();
        }

        private void CreateFootstepAudioPool()
        {
            _footstepSources = new AudioSource[footstepSourceCount];
            _footstepReverbs = new AudioReverbFilter[footstepSourceCount];

            for (int i = 0; i < footstepSourceCount; i++)
            {
                GameObject sourceObj = new GameObject("FootstepAudioSource_" + i);
                sourceObj.transform.SetParent(transform);
                sourceObj.transform.localPosition = Vector3.zero;

                AudioSource source = sourceObj.AddComponent<AudioSource>();
                source.playOnAwake = false;

                source.volume = _audioSource.volume;
                source.pitch = _audioSource.pitch;
                source.spatialBlend = _audioSource.spatialBlend;
                source.outputAudioMixerGroup = _audioSource.outputAudioMixerGroup;

                AudioReverbFilter reverb = sourceObj.AddComponent<AudioReverbFilter>();
                reverb.enabled = useReverb;
                reverb.reverbPreset = reverbPreset;

                _footstepSources[i] = source;
                _footstepReverbs[i] = reverb;
            }
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(
                transform.position.x,
                transform.position.y - GroundedOffset,
                transform.position.z
            );

            Grounded = Physics.CheckSphere(
                spherePosition,
                GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );
        }

        private void Move()
        {
            if (!canMove) return;

            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(
                _controller.velocity.x,
                0.0f,
                _controller.velocity.z
            ).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(
                    currentHorizontalSpeed,
                    targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate
                );

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = Vector3.zero;

            if (_input.move != Vector2.zero)
            {
                inputDirection = transform.right * _input.move.x +
                                 transform.forward * _input.move.y;
            }

            float fixedX = transform.position.x;
            float fixedZ = transform.position.z;

            _controller.Move(
                inputDirection.normalized * (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime
            );

            _controller.enabled = false;

            if (_input.move == Vector2.zero)
            {
                transform.position = new Vector3(fixedX, transform.position.y, fixedZ);
            }
            else
            {
                Vector3 delta = transform.position - new Vector3(fixedX, transform.position.y, fixedZ);
                float deltaRight = Vector3.Dot(delta, transform.right);
                float deltaForward = Vector3.Dot(delta, transform.forward);

                Vector3 allowedDelta = Vector3.zero;

                if (_input.move.x != 0f) allowedDelta += transform.right * deltaRight;
                if (_input.move.y != 0f) allowedDelta += transform.forward * deltaForward;

                transform.position = new Vector3(fixedX, transform.position.y, fixedZ) + allowedDelta;
            }

            _controller.enabled = true;
        }

        private void HandleFootsteps()
        {
            if (!Grounded ||
                !canMove ||
                _input.move == Vector2.zero ||
                footstepClips == null ||
                footstepClips.Length == 0)
            {
                _footstepTimer = 0f;
                return;
            }

            float interval = _input.sprint ? sprintFootstepInterval : footstepInterval;

            _footstepTimer += Time.deltaTime;

            if (_footstepTimer >= interval)
            {
                PlayFootstepSound();
                _footstepTimer = 0f;
            }
        }

        private void PlayFootstepSound()
        {
            if (_footstepSources == null ||
                _footstepSources.Length == 0 ||
                footstepClips == null ||
                footstepClips.Length == 0)
            {
                return;
            }

            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

            AudioSource source = _footstepSources[_currentFootstepSource];

            source.clip = clip;
            source.Play();

            _currentFootstepSource++;

            if (_currentFootstepSource >= _footstepSources.Length)
            {
                _currentFootstepSource = 0;
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                    _verticalVelocity = -2f;

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (_jumpTimeoutDelta >= 0.0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                    _fallTimeoutDelta -= Time.deltaTime;

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += Gravity * Time.deltaTime;
        }

        public void SetFootstepReverb(bool active)
        {
            useReverb = active;

            if (_footstepReverbs == null) return;

            foreach (AudioReverbFilter reverb in _footstepReverbs)
            {
                if (reverb != null)
                    reverb.enabled = active;
            }
        }

        public void SetReverbPreset(AudioReverbPreset preset)
        {
            reverbPreset = preset;

            if (_footstepReverbs == null) return;

            foreach (AudioReverbFilter reverb in _footstepReverbs)
            {
                if (reverb != null)
                    reverb.reverbPreset = preset;
            }
        }

        public void CanMove(bool m)
        {
            canMove = m;
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = Grounded ? transparentGreen : transparentRed;

            Gizmos.DrawSphere(
                new Vector3(
                    transform.position.x,
                    transform.position.y - GroundedOffset,
                    transform.position.z
                ),
                GroundedRadius
            );
        }
    }
}