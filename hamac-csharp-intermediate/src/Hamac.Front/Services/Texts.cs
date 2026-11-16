using System.Globalization;

namespace Hamac.Front.Services;

// Textes de l'interface en français et en anglais.
public class Texts
{
    private static readonly Dictionary<string, (string Fr, string En)> Entries = new()
    {
        ["Title"] = ("Demandes de congés", "Leave requests"),
        ["Employee"] = ("Salarié n°", "Employee no."),
        ["Balance"] = ("Solde", "Balance"),
        ["Days"] = ("jours", "days"),
        ["ShowBalance"] = ("Afficher le solde", "Show balance"),
        ["Filter"] = ("Statut", "Status"),
        ["All"] = ("Toutes", "All"),
        ["Id"] = ("N°", "No."),
        ["Start"] = ("Début", "Start"),
        ["End"] = ("Fin", "End"),
        ["DaysColumn"] = ("Jours", "Days"),
        ["Status"] = ("Statut", "Status"),
        ["Actions"] = ("Actions", "Actions"),
        ["Approve"] = ("Approuver", "Approve"),
        ["Refuse"] = ("Refuser", "Refuse"),
        ["NewRequest"] = ("Nouvelle demande", "New request"),
        ["Submit"] = ("Envoyer la demande", "Submit request"),
        ["NoRequest"] = ("Aucune demande.", "No requests."),
        ["Loading"] = ("Chargement…", "Loading…"),
        ["NotFound"] = ("Page introuvable.", "Page not found."),
        ["Pending"] = ("En attente", "Pending"),
        ["Approved"] = ("Approuvée", "Approved"),
        ["Refused"] = ("Refusée", "Refused")
    };

    public string Language { get; private set; } = "fr";

    public event Action? LanguageChanged;

    public string this[string key] =>
        Entries.TryGetValue(key, out var entry) ? (Language == "fr" ? entry.Fr : entry.En) : key;

    public CultureInfo Culture => CultureInfo.GetCultureInfo(Language == "fr" ? "fr-FR" : "en-GB");

    public string Status(string code) => this[code];

    public string Date(DateTime date) => date.ToString("d", Culture);

    public string Number(double value) => value.ToString("0.##", Culture);

    public void SetLanguage(string language)
    {
        Language = language;
        LanguageChanged?.Invoke();
    }
}
