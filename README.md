Guía de Instalación y Despliegue del Frontend
Introducción
Esta guía proporciona instrucciones sobre cómo instalar, configurar y desplegar la aplicación frontend de InventorySystem. 
La aplicación frontend se comunica con la API para mostrar y gestionar los productos en el inventario.


Configura la URL base de la API para que apunte al servidor de la API. El archivo puede ser algo como appsettings.json:

{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5021/api"
  }
}



