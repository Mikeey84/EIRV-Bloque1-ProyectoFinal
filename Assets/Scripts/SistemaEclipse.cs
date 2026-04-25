using UnityEngine;

public class SistemaEclipse : MonoBehaviour
{
    [Header("Referencias de Objetos")]
    public Transform sol;
    public Transform nave;
    public GameObject planeta;

    [Header("Configuración de Luz")]
    public Light luzSolar;
    public float radioDelSol = 5f; // Ajusta según el tamaño visual del sol

    [Space]
    public float intensidad0Rayos = 0.0f;  // Eclipse total
    public float intensidad1Rayo = 0.33f;  // Penumbra fuerte
    public float intensidad2Rayos = 0.66f; // Penumbra leve
    public float intensidad3Rayos = 1.0f;  // Luz total
    public LayerMask layer;
    [Header("Ajustes de Suavizado")]
    public float velocidadTransicion = 2.0f;

    private float intensidadObjetivo;

    void Update()
    {
        if (sol == null || nave == null || planeta == null || luzSolar == null) return;

        int rayosDespejados = 0;

        // 1. Rayo Central
        if (ChequearRayo(sol.position)) rayosDespejados++;

        // Calculamos vectores perpendiculares para los extremos
        Vector3 direccionAlSol = (sol.position - nave.position).normalized;
        Vector3 perpendicular = Vector3.Cross(direccionAlSol, Vector3.up).normalized * radioDelSol;

        // 2. Rayo Extremo Izquierdo
        if (ChequearRayo(sol.position + perpendicular)) rayosDespejados++;

        // 3. Rayo Extremo Derecho
        if (ChequearRayo(sol.position - perpendicular)) rayosDespejados++;

        // Asignar intensidad según cuántos rayos pasaron
        switch (rayosDespejados)
        {
            case 0: intensidadObjetivo = intensidad0Rayos; break;
            case 1: intensidadObjetivo = intensidad1Rayo; break;
            case 2: intensidadObjetivo = intensidad2Rayos; break;
            case 3: intensidadObjetivo = intensidad3Rayos; break;
        }

        // Aplicar suavizado
        luzSolar.intensity = Mathf.Lerp(luzSolar.intensity, intensidadObjetivo, Time.deltaTime * velocidadTransicion);
    }

    bool ChequearRayo(Vector3 puntoDestino)
    {
        Vector3 direccion = puntoDestino - nave.position;
        float distancia = direccion.magnitude;
        
        if (Physics.Raycast(nave.position, direccion, out RaycastHit hit, distancia, layer))
        {
            return false;
        }

        return true;
    }

    void OnDrawGizmos()
    {
        if (sol != null && nave != null)
        {
            Vector3 direccionAlSol = (sol.position - nave.position).normalized;
            Vector3 perpendicular = Vector3.Cross(direccionAlSol, Vector3.up).normalized * radioDelSol;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(nave.position, sol.position);
            Gizmos.DrawLine(nave.position, sol.position + perpendicular);
            Gizmos.DrawLine(nave.position, sol.position - perpendicular);
        }
    }
}