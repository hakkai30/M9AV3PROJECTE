# Mòdul MP09 - AV3
## Explorador web del universo de Rick and Morty

### Justificación y motivación
Este proyecto académico tiene como objetivo integrar el consumo de una API REST externa con la persistencia de datos en una base de datos local, aplicando el patrón MVC (Model-View-Controller) en .NET Core MVC. El enfoque permite combinar datos en tiempo real del universo de Rick and Morty con información propia de la aplicación, garantizando separación de responsabilidades, mantenibilidad y escalabilidad.

### Esquema de arquitectura
```text
+-----------+
|  Usuario  |
+-----+-----+
      |
      v
+-----------------------------+
| Controlador MVC             |
| (gestión de peticiones)     |
+-----------+-----------------+
            |
            +------------------------------+
            |                              |
            v                              v
+------------------------+      +---------------------------+
| API externa Rick&Morty |      | BBDD local SQLite         |
| (personajes/episodios) |      | (favoritos/usuarios)      |
+-----------+------------+      +-------------+-------------+
            |                                 |
            +---------------+-----------------+
                            |
                            v
                  +---------------------------+
                  | Vistas Razor/HTML/CSS     |
                  | (respuesta al usuario)     |
                  +---------------------------+
```

### Explicación detallada del código
La aplicación separa claramente la lógica de negocio según la fuente de datos:

- Personajes y Episodios: consumen la API oficial de Rick and Morty en tiempo real mediante peticiones HTTP desde la capa de servicios.
- Favoritos y gestión de usuarios: se gestionan contra la base de datos local SQLite para mantener persistencia y estado propio de la aplicación.

Para la serialización y deserialización de datos JSON se ha utilizado de forma explícita la librería Newtonsoft.Json, fundamental para mapear de forma fiable las respuestas de la API a los modelos del dominio. Además, el acceso a la aplicación está protegido mediante credenciales, restringiendo la gestión de recursos a usuarios autorizados.

### Acceso a Producción
- URL de producción: [http://hakkai01-001-site1.ntempurl.com/](http://hakkai01-001-site1.ntempurl.com/)
- Usuario: [TU_USUARIO]
- Contraseña: [TU_CONTRASEÑA]

### Accesibilidad y UX
Se han realizado validaciones de accesibilidad con la extensión WAVE sobre tres secciones en producción:

- Home: puntuación 10/10, 0 errores y 0 errores de contraste.
- Episodios: puntuación 9.9/10, 0 errores y 0 errores de contraste.
- Favoritos: puntuación 10/10, 0 errores y 0 errores de contraste.

Estos resultados evidencian un nivel alto de cumplimiento WCAG, sin incidencias críticas de accesibilidad. Las alertas detectadas por WAVE son de carácter preventivo y no implican errores de conformidad. Asimismo, se han implementado etiquetas aria-label dinámicas generadas con Razor para mejorar la navegación asistida tanto en vistas alimentadas por la API como en vistas con datos persistidos en SQLite.

#### Evidencias visuales (WAVE)
Capturas adjuntas de validación WAVE en producción:

![WAVE Home](docs/wave/PERSONATGES.jpeg)
![WAVE Episodios](docs/wave/EPISODIS.jpeg)
![WAVE Favoritos](docs/wave/FAVORITS.jpeg)

### Propuestas de mejora
- Implementar roles de usuario para controlar permisos de forma granular (por ejemplo, administrador, editor y usuario estándar).
- Incorporar filtros avanzados que combinen datos de la API externa y de la base de datos local para enriquecer la búsqueda y la exploración.

### Tecnologías utilizadas
- C#
- .NET Core MVC
- Razor
- HTML
- CSS
- JavaScript
- Newtonsoft.Json
- SQLite

---

## BLOQUE 2: Apartado para el Dossier de Projecte

### Análisis técnico de accesibilidad (WAVE) en entorno de producción
Se ha realizado una evaluación de accesibilidad con la herramienta WAVE en tres vistas funcionalmente representativas del sistema: Home, Episodios y Favoritos. El objetivo del análisis ha sido verificar el grado de cumplimiento de buenas prácticas WCAG en páginas que consumen datos desde fuentes distintas (API REST externa y base de datos local SQLite).

### Resultados observados
- Home: 10/10, con 0 errores y 0 errores de contraste.
- Episodios: 9.9/10, con 0 errores y 0 errores de contraste.
- Favoritos: 10/10, con 0 errores y 0 errores de contraste.

En las tres capturas no se detectan errores críticos ni incidencias de contraste, lo cual confirma una base de accesibilidad sólida en componentes estructurales, semánticos y visuales. Las alertas mostradas por WAVE se clasifican como advertencias preventivas y no constituyen fallos de conformidad WCAG.

### Interpretación académica de cumplimiento WCAG
Las puntuaciones obtenidas (10/10 y 9.9/10) permiten afirmar que el proyecto presenta un alto nivel de alineación con criterios WCAG, especialmente en los ejes de perceptibilidad y robustez. La ausencia de errores de contraste respalda la legibilidad del contenido para usuarios con diversidad visual, mientras que la ausencia de errores críticos sugiere una correcta implementación de estructura semántica y navegación asistida.

### Evidencia de implementación accesible en API y BBDD
Un aspecto relevante del proyecto es la coherencia accesible en ambos modelos de datos:

- Vistas con datos de API (por ejemplo, Episodios): se observa etiquetado accesible en elementos dinámicos renderizados desde respuestas externas.
- Vistas con datos de BBDD local (por ejemplo, Favoritos): se mantiene el mismo criterio de accesibilidad en contenido persistido localmente.

En ambos casos, se emplean etiquetas aria-label generadas dinámicamente con Razor, lo que permite que la semántica accesible se adapte al contenido real mostrado en tiempo de ejecución. Esta estrategia mejora la experiencia con tecnologías de asistencia y demuestra una integración correcta entre accesibilidad, lógica de presentación y procedencia de datos.

### Conclusión
La validación técnica mediante WAVE evidencia que la aplicación alcanza un nivel excelente de accesibilidad para un proyecto académico MVC, con rendimiento estable en páginas de distinta naturaleza funcional y origen de datos. Como línea de mejora futura, se recomienda mantener auditorías periódicas y revisar alertas preventivas para consolidar aún más la calidad de la interfaz.
