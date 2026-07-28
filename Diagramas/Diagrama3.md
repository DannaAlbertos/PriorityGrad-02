# Diagrama C4 Nivel 3 — Componentes

**Para quién es:** Desarrolladores encargados de la implementación del código.
**Qué responde:** Describe la estructura interna de la aplicación aplicando Arquitectura Hexagonal y separación de responsabilidades.

```mermaid
flowchart TD
    subgraph WebApp [" PriorityGrad Web App - Arquitectura Hexagonal"]
        
        UI[" Capa de Presentación\n(Controllers / Views Razor)"]
        
        subgraph Core [" Capa de Dominio (Core)"]
            UseCase[" Casos de Uso\n(TaskPriorityService)"]
            Ports[" Puertos\n(ITaskRepository)"]
        end
        
        Infra["🏗 Capa de Infraestructura\n(SQLTaskRepository)"]
    end

    DB[(" Base de Datos\n(SQL Server)")]

    UI -- "Invoca lógica de negocio" --> UseCase
    UseCase -- "Define interfaces" --> Ports
    Infra -. "Implementa (Inyección de dependencias)" .-> Ports
    Infra -- "Realiza consultas (EF Core)" --> DB

    style Core fill:#f8e1e7,stroke:#5a4a4a,stroke-width:2px,color:#5a4a4a
    style UI fill:#ffffff,stroke:#d4a5a5,stroke-width:2px
    style Infra fill:#ffffff,stroke:#d4a5a5,stroke-width:2px
```