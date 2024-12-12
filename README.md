# Demo of Document Transformer Issue

This project demonstrates an issue with document transformer in regards to the security schema flows.

Build the project using `dotnet build` and check the generated [Demo.json](Demo.json) file.

expected:

```json
"securitySchemes": {
    "Bearer": {
        "flows": {
            "password": {
                "tokenUrl": "https://localhost",
                "scopes": {
                    "api": "API access"
                }
            }
        },
        "type": "http",
        "scheme": "Bearer"
    }
}
```

actual:

```json
"securitySchemes": {
    "Bearer": {
        "type": "http",
        "scheme": "Bearer"
    }
}
```
