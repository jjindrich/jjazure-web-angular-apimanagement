# JJ AngularWeb using Angular calling back-end API with Azure API management

SinglePage Angular web application calling back-end rest API published with API management. Backend API is secured using bearer tokens from Azure Active Directory .

**TODO**:

- Multiple API services (now only Books) - check https://github.com/jjindrich/jjazure-web-angular-apimanagement/pull/1
- Host API in Docker using Azure Mesh

**Design for development**:

- all API services will be published with public IP
- API services publish with Azure API Management

**Design for production**:

- all API services are deployed into virtual network
- API services publish with Azure API Management connected to virtual network

 Why use Azure API management - [Direct communication vs API management](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/architect-microservice-container-applications/direct-client-to-microservice-communication-versus-the-api-gateway-pattern)

- [Protect your API](https://docs.microsoft.com/en-us/azure/api-management/transform-api)
- [Combine with Azure Application Gateway](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-integrate-internal-vnet-appgateway)

*If you will not use API management, you have to implement security checks on your API services directly.*

## Create frontend web

### Create Angular project with Visual Studio Code

I created SinglePage Angular (SPA) web project

How to create new Angular project (with routing) - [Using Angular in Visual Studio Code](https://code.visualstudio.com/docs/nodejs/angular-tutorial)

Tutorial for Angular - [Angular Tutorial](https://angular.io/tutorial/toh-pt0)

Use Material Design - [Angular Material](https://material.angular.io/guide/getting-started)

Run web on localhost, open browser with http://localhost:4200

```bash
cd jjweb
ng serve --open
```

### Add Azure Active Directory to Angular web project

Make sure you have your Azure Active Directory, my is jjdev.onmicrosoft.com

Add Azure Active Directory Sing-in - [Angular use ADAL](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v1-angularjs-spa)

Example for Angular6 - [Adal-Angular6](https://github.com/benbaran/adal-angular6-example)

Steps to configure Azure Active Directory

1. Add App registration - type https://localhost:42000/
2. Change manifest to oauth2AllowImplicitFlow: true
3. Grant permissions for application

```bash
npm install --save adal-angular4
```

Change source code

1. Change your tenant and clientId in environment settings - yourdomain.onmicrosoft.com and applicationId

Run web on HTTPS localhost (required for claim based auth), open browser with https://localhost:4200

```bash
cd jjweb
ng serve --ssl --open
```

Open browser and check JWT (JSON Web Token) token after successful sign-in. Decode token with [jwt.io](https://jwt.io/).

### Use Authorization to call API service

Code is adding Authorization header with JWT token.

```js
    const httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')      
      .set('Authorization', 'Bearer ' + adalService.userInfo.token); 
```

## Create API backend

I prepared two options how to create API backend

1. Using platform service Azure API App (Web App) - easier to deploy
2. Using microservice cluster Azure ServiceFabric - more complex to create cluster but with lof of advantages from deployment perspective

### Create API project with Visual Studio hosted in Azure Web App

I created DotNet Core API project jjapi and published to Azure API App.

How to add Swagger - [NSwag](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml)

Configure Azure API App (Web App)

1. Swagger link must be updated on API App - API Definition blade - type https://jjapiapp.azurewebsites.net/swagger/v1/swagger.json
2. You will get this API URL: https://jjapiapp.azurewebsites.net/api/books
3. Update CORS url on WebApp based on your web client jjweb, like http://localhost:4200
4. Update SPA project with this url, file main.ts

### Create API project with Visual Studio hosted in Azure ServiceFabric

I created Stateless ASP.Net Core API **project jjapisf with API books** and published to Azure ServiceFabric jjsf.westeurope.cloudapp.azure.com

[Hosting ASP.Net Core in ServiceFabric](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication-aspnetcore)

Warning: now using Kestrel, limited for multiple services running on same port.

Next I created **second API service called jjapisf2 with API orders**

If you want to use port sharing, you have to use HttpSys listener - [Using unique service URLs](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication-aspnetcore#using-unique-service-urls)

- add nuget package Microsoft.ServiceFabric.AspNetCore.HttpSys
- change in code Kestrel to HttpSysCommunicationListener
- add your url, like jjapisf1

**Configure Azure ServiceFabic**

1. Create with published ports 80, 443
2. Import PFX/PEM certificate into your user certificate store
3. Publish Visual Studio project jjapisf to Azure ServiceFabric

Your services will be available on http://jjsf.westeurope.cloudapp.azure.com/jjapisf1/api/books and http://jjsf.westeurope.cloudapp.azure.com/jjapisf2/api/values

How to communicate between services internally in ServiceFabric - [ServiceFabric internal communication](https://dzimchuk.net/implementing-a-rest-client-for-internal-communication-in-service-fabric/)

How to generate proxy for APIs [NSwag](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag)

## Publish API backend with Azure API management

Provision Azure API Management, my is jjapi

- Developer plan is limited, you cannot connect to virtual network for backend services
- Premium plan is production ready, you can connect to virtual network for backend services

### Publish Books API with Azure API management

Open API Management service add new API (one of them)

1. Open API specification (for ServiceFabric deployment) - type http://jjsf.westeurope.cloudapp.azure.com/jjapisf1/api-specification.json
2. Open API App - select API App Azure resource with configured API definition
3. Create new plan Free (for testing) - will not require subscription
4. Setup policy for CORS - click Add policy CORS for Inbound processing or [CORS](https://docs.microsoft.com/en-us/azure/api-management/api-management-cross-domain-policies)
5. Check API settings for URL scheme to HTTPs or Both
6. Update SPA project with this url, file main.ts - type https://jjapi.azure-api.net/books/api/books

![API management Books API](media/api-books-design.png)

![API management Books API](media/api-books-settings.png)

### Secure Books API with Azure Active Directory

[Protect API with Azure AD](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-protect-backend-with-aad)

[Validate JWT](https://docs.microsoft.com/en-us/azure/api-management/api-management-access-restriction-policies#ValidateJWT)

Configuration of API management

1. Follow instructions to enable OAuth2
2. Secure API service - enable OAuth2 and setup policy to check JWT token, my service https://jjapi.azure-api.net/BooksSecure/api/Books
3. Change url in main.ts to access Secured API service

Inbound policy to check JWT - check if using ClientId of Developer Console or jjweb

```xml
        <validate-jwt header-name="Authorization" failed-validation-httpcode="401" failed-validation-error-message="Unauthorized. JJ Access token is missing or invalid.">
            <openid-config url="https://login.microsoftonline.com/jjdev.onmicrosoft.com/.well-known/openid-configuration" />
            <required-claims>
                <claim name="aud" match="any">
                    <value>01022e81-e7ed-4498-a898-09b16d1c2b7e</value>
                    <value>e1bb9975-c60b-43e1-8d14-00b8fad78029</value>
                </claim>
            </required-claims>
        </validate-jwt>
```

You can test it from Developer Console - Authorization value is generated after successful login.

![API management Books Secure API](media/api-bookssecure-design.png)

### Create Mock response service

You can start with creating API service as mock and implement service later.

Options to create mock response

- use [Mock response](https://docs.microsoft.com/en-us/azure/api-management/api-management-advanced-policies#mock-response) for just response
- user [Return response](https://docs.microsoft.com/en-us/azure/api-management/api-management-advanced-policies#ReturnResponse) for complete response

Sample for JSON response

```xml
    <outbound>
        <base />
        <return-response>
            <set-status code="200" reason="OK" />
            <set-header name="content-type" exists-action="override">
                <value>application/json; charset=utf-8</value>
            </set-header>
            <set-body>[{"id":1,"name":"Book1"},{"id":2,"name":"Book2"},{"id":3,"name":"Book3"}]</set-body>
        </return-response>
    </outbound>
```

### API guides

[Debug API](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-api-inspector)

[Version API](https://docs.microsoft.com/en-us/azure/api-management/api-management-get-started-publish-versions)

## Communication with database

Connection resiliency for Entity Framework core: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency