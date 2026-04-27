Mòdul MP09 - AV3
================

Explorador web del universo de Rick and Morty
---------------------------------------------

### Justificación y motivación

Este proyecto académico tiene como objetivo integrar el consumo de una API REST externa con la persistencia de datos en una base de datos local, aplicando el patrón MVC (Model-View-Controller) en .NET Core MVC. El enfoque permite combinar datos en tiempo real del universo de Rick and Morty con información propia de la aplicación, garantizando separación de responsabilidades, mantenibilidad y escalabilidad.

### Esquema de arquitectura

```text
+-----------+
| Usuario   |
+-----+-----+
      |
      v
+-----------------------------+
| Controlador MVC            |
| (gestión de peticiones)    |
+-----------+----------------+
            |
            v
+------------------------+       +---------------------------+
| API externa Rick&Morty |       | BBDD local SQLite        |
| (personajes/episodios) |       | (favoritos)              |
+-----------+------------+       +-------------+------------+
            |                              |
            +---------------+--------------+
                            v
+---------------------------+
| Vistas Razor/HTML/CSS     |
| (respuesta al usuario)    |
+---------------------------+
```

### Explicación detallada del código

La aplicación separa claramente la lógica de negocio según la fuente de datos:

- Personatges i episodis: es carreguen des de l’API oficial de Rick and Morty en temps real mitjançant peticions HTTP des de la capa de serveis.
- Favorits: es gestionen contra la base de dades local SQLite per mantenir un estat propi de l’aplicació.

Para la serialización y deserialización de datos JSON se ha utilizado de forma explícita la librería **Newtonsoft.Json**, fundamental para mapear de forma fiable las respuestas de la API a los modelos del dominio.

### Compliment de requisits MP09 (AV3)

Aquest projecte compleix el requisit de **“codificar mínim diferents rutes al controlador, models i vistes que mostri dades de consum d’un servei Web i consultes a BBDD”** del MP09. [file:2]

- **Controladors i rutes**
  - `HomeController`: accions `Index` i `Error`.
  - `RickAndMortyController`: accions `Characters`, `Character`, `Episodes`, `Episode`, `Favorites`, `AddFavorite` i `RemoveFavorite`.
  - En total, diverses rutes funcionals que responen a URLs diferents i mostren informació distinta.

- **Models i ViewModels**
  - Models de domini: `Character`, `Episode`, `FavoriteCharacter`, `ApiResponse<T>` i `Location`.
  - ViewModels: `HomeViewModel`, `CharacterListViewModel`, `CharacterDetailViewModel`.
  - S’utilitzen per separar clarament la informació de l’API, la BBDD i la presentació a les vistes.

- **Vistes Razor**
  - Vistes fortament tipades per a: Home, llistat de personatges, detall de personatge, llistat d’episodis, detall d’episodi i favorits.
  - Cada acció de controlador té associada una vista que mostra les dades corresponents.

- **Consum de servei Web**
  - Servei `RickAndMortyService` que crida l’API oficial (`https://rickandmortyapi.com/api`) amb HttpClient.
  - Es consumeixen recursos de personatges i episodis, amb paginació i filtres opcionals.

- **Consultes a BBDD**
  - `AppDbContext` amb DbSet de `FavoriteCharacter` i configuració SQLite.
  - Les accions `Favorites`, `AddFavorite` i `RemoveFavorite` treballen directament contra la base de dades per llegir, afegir i eliminar favorits.

Aquests punts deixen explícit que el projecte utilitza **MVC**, múltiples rutes, models i vistes, consum d’un servei web i consultes a una BBDD SQLite, tal com demana l’enunciat. [file:2]

### Acceso a Producción

- URL de producción: http://hakkai01-001-site1.ntempurl.com/
- Usuari: (veure credencials indicades pel professor o per l’alumne en l’entrega)
- Contrasenya: (veure credencials indicades pel professor o per l’alumne en l’entrega)

### Accesibilidad y UX

Se han realizado validaciones de accesibilidad con la extensión WAVE sobre tres secciones en producción:

- Home: puntuación 10/10, 0 errores y 0 errores de contraste.
- Episodios: puntuación 9.9/10, 0 errores y 0 errores de contraste.
- Favoritos: puntuación 10/10, 0 errores y 0 errores de contraste.

Estos resultados evidencian un nivel alto de cumplimiento WCAG, sin incidencias críticas de accesibilidad. Las alertas detectadas por WAVE son de carácter preventivo y no implican errores de conformidad. Asimismo, se han implementado etiquetas `aria-label` dinámicas generadas con Razor para mejorar la navegación asistida tanto en vistas alimentadas por la API como en vistas con datos persistidos en SQLite.

#### Evidencias visuales (WAVE)

Capturas adjuntas de validación WAVE en producción:

- WAVE Home: `docs/wave/PERSONATGES.jpeg`
- WAVE Episodios: `docs/wave/EPISODIS.jpeg`
- WAVE Favoritos: `docs/wave/FAVORITS.jpeg`

### Propuestas de mejora

- Implementar rols d’usuari per controlar permisos de forma més granular (administrador, usuari estàndard, etc.).
- Incorporar filtres avançats que combinin dades de l’API externa i de la base de dades local per enriquir la cerca i l’exploració.
- Afegir més proves d’accessibilitat automàtiques i manuals per mantenir el nivell de qualitat a llarg termini.

### Tecnologías utilizadas

- C#
- .NET Core MVC
- Razor
- HTML
- CSS
- JavaScript
- Newtonsoft.Json
- SQLite

BLOQUE 2: Apartado para el Dossier de Projecte
----------------------------------------------

### Análisis técnico de accesibilidad (WAVE) en entorno de producción

Se ha realizado una evaluación de accesibilidad con la herramienta WAVE en tres vistas funcionalmente representativas del sistema: Home, Episodios y Favoritos. El objetivo del análisis ha sido verificar el grado de cumplimiento de buenas prácticas WCAG en páginas que consumen datos desde fuentes distintas (API REST externa y base de datos local SQLite).

### Resultados observados

- Home: 10/10, con 0 errores y 0 errores de contraste.
- Episodios: 9.9/10, con 0 errores y 0 errores de contraste.
- Favoritos: 10/10, con 0 errores y 0 errores de contraste.

En las tres capturas no se detectan errores críticos ni incidencias de contraste, lo cual confirma una base de accesibilidad sólida en componentes estructurales, semánticos y visuales. Las alertas mostradas por WAVE se clasifican como advertencias preventivas y no constituyen fallos de conformidad WCAG.

### Interpretación académica de cumplimiento WCAG

Las puntuaciones obtenidas permiten afirmar que el proyecto presenta un alto nivel de alineación con criterios WCAG, especialmente en los ejes de perceptibilidad y robustez. La ausencia de errores de contraste respalda la legibilidad del contenido para usuarios con diversidad visual, mientras que la ausencia de errores críticos sugiere una correcta implementación de estructura semántica y navegación asistida.

### Evidencia de implementación accesible en API y BBDD

Un aspecto relevante del proyecto es la coherencia accesible en ambos modelos de datos:

- Vistas con datos de API (por ejemplo, Episodios): se observa etiquetado accesible en elementos dinámicos renderizados desde respuestas externas.
- Vistas con datos de BBDD local (por ejemplo, Favoritos): se mantiene el mismo criterio de accesibilidad en contenido persistido localmente.

En ambos casos, se emplean etiquetas `aria-label` generadas dinámicamente con Razor, lo que permite que la semántica accesible se adapte al contenido real mostrado en tiempo de ejecución.

### Conclusión

La validación técnica mediante WAVE evidencia que la aplicación alcanza un nivel excelente de accesibilidad para un proyecto académico MVC, con rendimiento estable en páginas de distinta naturaleza funcional y origen de datos. Como línea de mejora futura, se recomienda mantener auditorías periódicas y revisar alertas preventivas para consolidar aún más la calidad de la interfaz.
