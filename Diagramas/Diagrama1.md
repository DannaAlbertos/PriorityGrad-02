# Diagrama C4 Nivel 1 — Contexto

**Para quién es:** Usuarios no técnicos, profesores evaluadores y stakeholders.
**Qué responde:** Muestra el panorama general del sistema y su interacción directa con el usuario final.

```mermaid
flowchart TD
    Usuario[" Estudiante\n(Usuario final)"]
    Sistema[" PriorityGrad\n(Sistema de Gestión Académica)"]

    Usuario -- "Registra tareas, fechas límite y consulta prioridades" --> Sistema
    
    style Sistema fill:#d4a5a5,stroke:#5a4a4a,stroke-width:2px,color:#fff
```