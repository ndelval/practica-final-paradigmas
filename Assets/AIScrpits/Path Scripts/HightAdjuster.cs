using UnityEngine;

[ExecuteInEditMode]
public class AdjustNodeHeight : MonoBehaviour
{
    public Transform parentTransform;  // El padre que contiene todos los nodos
    public LayerMask trackLayer;       // Capa de la pista
    public float heightOffset = 1.0f;  // Distancia de altura deseada desde la pista

    private void OnValidate()
    {
        AdjustNodeHeights();
    }

    private void AdjustNodeHeights()
    {
        if (parentTransform == null)
        {
            Debug.LogWarning("No se ha asignado parentTransform.");
            return;
        }

        // Obtener todos los hijos de parentTransform (incluidos los nodos)
        Transform[] pathTransforms = parentTransform.GetComponentsInChildren<Transform>();

        // Ajustar cada nodo en función de la altura de la pista
        foreach (Transform node in pathTransforms)
        {
            if (node != parentTransform)  // Ignorar el padre mismo
            {
                RaycastHit hit;

                // Realizar un Raycast desde arriba hacia abajo
                if (Physics.Raycast(node.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, trackLayer))
                {
                    // Colocar el nodo a una distancia de heightOffset sobre la pista
                    node.position = hit.point + Vector3.up * heightOffset;
                }
                else
                {
                    Debug.LogWarning($"El nodo {node.name} no está sobre la pista.");
                }
            }
        }
    }
}
