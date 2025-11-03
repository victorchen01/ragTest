# RAG Data Service

A Retrieval-Augmented Generation (RAG) system that uses semantic search to find similar text documents based on embeddings. This project combines a .NET API with a Python embedding service to enable intelligent document search.

## Architecture

```
Postman/Client → .NET API (Port 5005) → Python Embedding Service (Port 8000)
                                    ↓
                            MongoDB Atlas
```

- **.NET Service**: Main API that handles requests, manages MongoDB, and performs similarity calculations
- **Python Service**: Generates text embeddings using SentenceTransformers (all-MiniLM-L6-v2 model)
- **MongoDB Atlas**: Stores text documents with their vector embeddings

## Features

- ✅ Add single text entries with automatic embedding generation
- ✅ Bulk add multiple entries at once
- ✅ Query documents using semantic similarity search
- ✅ Returns top 3 most relevant matches with similarity scores
- ✅ Cosine similarity calculation for accurate matching

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Python 3.11+](https://www.python.org/downloads/)
- MongoDB Atlas account (or local MongoDB instance)
- Postman (or any HTTP client) for testing

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd ragTest
```

### 2. Configure MongoDB

1. Copy the example configuration file:
   ```bash
   cd RAGDataService
   copy appsettings.json.example appsettings.json
   ```

2. Update `appsettings.json` with your MongoDB Atlas connection string:
   ```json
   {
     "MongoDB": {
       "ConnectionString": "mongodb+srv://your-username:your-password@cluster.mongodb.net/?retryWrites=true&w=majority",
       "DatabaseName": "RAG_DB"
     }
   }
   ```

3. **Important**: Make sure your IP address is whitelisted in MongoDB Atlas (Network Access section)

### 3. Set Up Python Environment

1. Create a virtual environment:
   ```bash
   python -m venv venv
   ```

2. Activate the virtual environment:
   ```bash
   # Windows PowerShell
   .\venv\Scripts\Activate.ps1
   
   # Windows CMD
   venv\Scripts\activate.bat
   
   # macOS/Linux
   source venv/bin/activate
   ```

3. Install Python dependencies:
   ```bash
   pip install flask sentence-transformers "numpy<2"
   ```

   **Note**: NumPy 2.x has compatibility issues with PyTorch. Use NumPy < 2.0.

### 4. Run the Services

#### Terminal 1: Python Embedding Service
```bash
python embedding_api.py
```
The service will start on `http://localhost:8000`

#### Terminal 2: .NET API Service
```bash
cd RAGDataService
dotnet run
```
The service will start on `http://localhost:5005`

## API Endpoints

### Add Single Entry
**POST** `/RAG/datasync`

Add a single text entry to the database.

**Request Body:**
```json
"Your text here"
```

**Response:**
```json
"Data synced successfully"
```

### Add Multiple Entries (Bulk)
**POST** `/RAG/datasync/bulk`

Add multiple text entries at once.

**Request Body:**
```json
[
  "First text entry",
  "Second text entry",
  "Third text entry"
]
```

**Response:**
```json
{
  "message": "3 entries added successfully"
}
```

### Query Documents
**POST** `/RAG/dataquery`

Search for semantically similar documents.

**Request Body:**
```json
"What is machine learning?"
```

**Response:**
```json
[
  {
    "text": "Machine learning is a subset of artificial intelligence...",
    "score": 0.89
  },
  {
    "text": "Deep learning uses multiple layers of neural networks...",
    "score": 0.76
  },
  {
    "text": "Natural language processing allows computers to understand...",
    "score": 0.72
  }
]
```

## Testing with Postman

### Example Requests

1. **Add an entry:**
   - Method: `POST`
   - URL: `http://localhost:5005/RAG/datasync`
   - Headers: `Content-Type: application/json`
   - Body (raw JSON): `"Machine learning is a subset of artificial intelligence."`

2. **Query entries:**
   - Method: `POST`
   - URL: `http://localhost:5005/RAG/dataquery`
   - Headers: `Content-Type: application/json`
   - Body (raw JSON): `"What is AI?"`

3. **Bulk add entries:**
   - Method: `POST`
   - URL: `http://localhost:5005/RAG/datasync/bulk`
   - Headers: `Content-Type: application/json`
   - Body (raw JSON):
   ```json
   [
     "First entry",
     "Second entry",
     "Third entry"
   ]
   ```

## How It Works

1. **Adding Data:**
   - Text is sent to the .NET API
   - API forwards text to Python service for embedding generation
   - Python uses SentenceTransformer to convert text → vector (384 dimensions)
   - Both text and embedding are stored in MongoDB

2. **Querying:**
   - Query text is converted to an embedding vector
   - All documents are retrieved from MongoDB
   - Cosine similarity is calculated between query and each document
   - Top 3 most similar documents are returned with scores

## Project Structure

```
ragTest/
├── RAGDataService/          # .NET API
│   ├── Controllers/         # API endpoints
│   ├── Services/           # Business logic (MongoDB, Embeddings)
│   ├── Models/             # Data models
│   └── appsettings.json    # Configuration (not in git)
├── embedding_api.py        # Python embedding service
├── venv/                   # Python virtual environment (not in git)
└── README.md              # This file
```

## Troubleshooting

### Python NumPy Errors
If you see `RuntimeError: Numpy is not available`, downgrade NumPy:
```bash
pip install "numpy<2"
```

### MongoDB Connection Issues
- Verify your connection string in `appsettings.json`
- Check that your IP is whitelisted in MongoDB Atlas
- Ensure your MongoDB user has read/write permissions

### Python Service Not Found
- Ensure Python service is running on port 8000
- Check firewall settings
- Verify the Python service logs for errors

### .NET Service Port Conflicts
- Default port is 5005 (HTTP) and 7121 (HTTPS)
- Check `launchSettings.json` to modify ports
- Ensure no other services are using these ports

## Security Notes

- ⚠️ **Never commit `appsettings.json`** - it contains sensitive MongoDB credentials
- Use environment variables or .NET User Secrets for production
- The example file (`appsettings.json.example`) is safe to commit

## Technologies Used

- **.NET 10.0**: Web API framework
- **Python 3.11+**: Embedding service
- **Flask**: Python web framework
- **SentenceTransformers**: Text embedding model (all-MiniLM-L6-v2)
- **MongoDB**: Document database
- **MongoDB.Driver**: .NET MongoDB client

## License

[Your License Here]

