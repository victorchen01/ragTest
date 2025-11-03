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

### 3. Set Up Python Environment

1. Create a virtual environment:
   ```bash
   python -m venv venv
   ```

2. Activate the virtual environment:
   ```bash
   # Windows PowerShell
   .\venv\Scripts\Activate.ps1

3. Install Python dependencies:
   ```bash
   pip install flask sentence-transformers "numpy<2"
   ```

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
"<query>?"
```
