# Hamac

Application interne de gestion des congés : les salariés consultent leur solde
et déposent leurs demandes, les managers les approuvent ou les refusent.

## Démarrer

Prérequis : SDK .NET 10.

    dotnet run --project src/Hamac.Api      # API sur http://localhost:5110
    dotnet run --project src/Hamac.Front    # Front-end sur http://localhost:5200

La base SQLite de démonstration est réinitialisée à chaque démarrage de l'API.

## Configuration

| Clé | Fichier | Rôle |
| --- | --- | --- |
| ConnectionStrings:Hamac | src/Hamac.Api/appsettings.json | Base de données |
| Front:Origins | src/Hamac.Api/appsettings.Development.json | Origines autorisées par CORS |
| Api:BaseUrl | src/Hamac.Front/wwwroot/appsettings.json | Adresse de l'API |

## Architecture

- src/Hamac.Api : contrôleurs (HTTP uniquement), LeaveService (règles métier),
  politiques de congés (une classe par type de contrat), HamacDbContext (EF Core).
- src/Hamac.Front : Blazor WebAssembly. Les écrans dépendent de ILeaveApi ;
  HttpLeaveApi appelle l'API, MockLeaveApi fournit des données simulées.
- tests/Hamac.Tests : tests xUnit (API et politiques).

Ajouter un type de contrat : une constante dans ContractTypes, une classe qui
implémente ILeaveEntitlementPolicy, une ligne dans Program.cs, un test unitaire.

## API

Contrat complet : http://localhost:5110/openapi/v1.json (en développement).
Exemples prêts à l'emploi : src/Hamac.Api/Hamac.Api.http.

| Verbe | Route | Rôle |
| --- | --- | --- |
| GET | api/employees/{id}/balance | Solde d'un salarié |
| GET | api/leave-requests?status= | Liste des demandes, filtrable par statut |
| GET | api/leave-requests/{id} | Détail d'une demande |
| POST | api/leave-requests | Dépôt d'une demande (201) |
| POST | api/leave-requests/{id}/approval | Approbation (409 si déjà traitée) |
| POST | api/leave-requests/{id}/refusal | Refus (409 si déjà traitée) |

## Tests

    dotnet test

## Règles métier et limites connues

Les règles de calcul des droits sont validées par Samuel Okafor (RH) :
ne modifiez pas une règle sans son accord.

- Les jours fériés ne sont pas déduits du nombre de jours ouvrés.
- Les messages d'erreur de l'API sont en français, quelle que soit la langue de l'écran.
- La politique des CDD renvoie un droit négatif pour une date d'entrée future (à confirmer avec Samuel).
