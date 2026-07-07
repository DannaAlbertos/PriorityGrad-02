# ADR-01: [Diagrama C4]

| Campo  | Valor |
|--------|-------|
| Autor  | Danna Albertos |
| Fecha  | 07/07/2026 |
| Estado | `Propuesto`|

---

## Contexto

Para este proyecto estoy construyendo PriorityGrad que es un organizador de tareas diseñado para estudiantes universitarios y/o de cualquier grado de escuela que enfrentan una alta carga de tareas y proyectos además de mantener una vida personal y profesional. Este sistema resuelve de manera directa el problema del sesgo de urgencia, el cual provoca que los alumnos dediquen su tiempo a actividades sencillas o de última hora en lugar de enfocarse en los entregables que realmente tienen un impacto crítico en su promedio final. Como restricciones principales para el diseño de este software, debo considerar que me encuentro en el tercer cuatrimestre de la ingeniería, por lo que requiero una estructura técnica que sea sólida y que demuestre un control claro sobre la separación de responsabilidades, pero sin añadir capas de infraestructura complejas que pongan en riesgo la entrega del proyecto en el límite de tiempo de menos de cuatro meses que dura el periodo escolar.

---

## Diagrama

![Diagrama del sistema]

flowchart TD
    Usuario[" Estudiante\n(Usuario final)"]
    Sistema[" PriorityGrad\n(Sistema de Gestión Académica)"]

    Usuario -- "Registra tareas, fechas límite y consulta prioridades" --> Sistema
    
    style Sistema fill:#d4a5a5,stroke:#5a4a4a,stroke-width:2px,color:#fff

