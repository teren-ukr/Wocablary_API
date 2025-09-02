# Wocabulary Backend API

This project serves as the backend for "Wocabulary," an innovative dictionary web application. It leverages the **Google Gemini API** to automatically enrich words with additional data.

The core idea is simple: a user can add a word, and the system, powered by a large language model, automatically generates its translation, a detailed description, an interesting story about its origin, its word type, and even a link to a relevant image.

---

## 🛠️ Technologies Used

* **.NET 8** - The development framework.
* **ASP.NET Core** - For building the API.
* **Google Gemini API** - Integration for data enrichment.
* **Entity Framework Core** - For database interaction.

---

## 💡 Key Features

* **Word Management (CRUD)**: A full set of functionalities to create, read, update, and delete words in the database.
* **Automatic Data Enrichment**: Upon an API request, a word is automatically enriched with fields populated by AI.
* **Scalable Architecture**: The project is built with a clean architecture approach, making it easy to maintain and extend.

### Data Model: `Word`

* `WordText`: **(required)** The original word.
* `WordTranslation`: The AI-generated translation.
* `WordDescription`: A detailed description.
* `WordStory`: An interesting story or etymology.
* `WordType`: The part of speech (e.g., noun, verb).
* `WordImageURL`: A link to a relevant image.

---

## 🚀 How to Run the Project

### ⚙️ Prerequisites

* **.NET SDK 8** or a newer version installed.
* **A Google Gemini API key**. This key needs to be added to the project's configuration.

### 🖥️ Instructions

1.  Clone the repository:
    ```bash
    git clone [[https://github.com/your_username/your_repo_name.git](https://github.com/your_username/your_repo_name.git)](https://github.com/teren-ukr/Wocablary_API.git)
    cd your_repo_name
    ```
2.  Add your Gemini API key to the `appsettings.json` file:
    ```json
    "GeminiApiSettings": {
      "ApiKey": "YOUR_GEMINI_API_KEY"
    }
    ```
3.  Restore dependencies and run the project:
    ```bash
    dotnet restore
    dotnet run
    ```
The project will start on a local server, typically at `https://localhost:7121` or as configured in your settings.

After that you need to install frontend side. Go to this repository --> https://github.com/teren-ukr/wocab-frontend <--

---

## 🗺️ API Documentation

### `TestController`

* **`POST /api/Test/save`**: Saves a test word.
* **`GET /api/Test/getAll`**: Retrieves a list of all words.
* **`POST /api/Test/AddWord`**: Adds a new word to the database. Accepts an `AddWordRequest` object in the request body.
* **`DELETE /api/Test/{id}`**: Deletes a word by its ID.
* **`PUT /api/Test/{id}`**: Updates a word by its ID. Accepts an `UpdateWordRequest` object in the request body.
* **`GET /api/Test/{id}`**: Retrieves a word by its ID.

### `GeminiAIController`

* **`POST /api/GeminiAI/enrich-word`**: Enriches a word using the Gemini API.
    * **Example Request**:
        ```json
        {
          "WordText": "sunshine"
        }
        ```
    * **Example Response**:
        ```json
        {
          "WordText": "sunshine",
          "WordTranslation": "сонячне світло",
          "WordDescription": "Light that comes from the sun, or direct rays of the sun...",
          // ... other fields
        }
        ```

---
