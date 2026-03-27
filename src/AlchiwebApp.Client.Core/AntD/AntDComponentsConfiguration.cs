using AntDesign;

namespace AlchiwebApp.Client.Core.AntD;

public class AntDComponentsConfiguration
{
    private TableLocale _tableLocaleFr = new TableLocale
    {
        FilterTitle = "Filtrer",
        FilterConfirm = "OK",
        FilterEmptyText = "Vide",
        FilterReset = "Réinitialiser",
        FilterOptions = new FilterOptionsLocale
        {
            True = "vrai",
            False = "faux",
            And = "Et",
            Or = "Ou",
            Equals = "est égal à",
            NotEquals = "n'est pas égal à",
            Contains = "contient",
            NotContains = "ne contient pas",
            StartsWith = "commence par",
            EndsWith = "fini par",
            GreaterThan = "plus grand que",
            LessThan = "plus petit que",
            GreaterThanOrEquals = "plus grand ou égal à",
            LessThanOrEquals = "plus petit ou égal à",
            IsNull = "est nul",
            IsNotNull = "n'est pas nul",
            TheSameDateWith = "est la même date que",
            Between = "entre"
        },
        SelectAll = "Sélectionner la page actuelle",
        SelectInvert = "Inverser la sélection de la page actuelle",
        SelectionAll = "Sélectionner toutes les données",
        SortTitle = "Trier",
        Expand = "Développer la ligne",
        Collapse = "Réduire la ligne",
        TriggerDesc = "Trier par ordre décroissant",
        TriggerAsc = "Trier par ordre croissant",
        CancelSort = "Annuler le tri"
    };
    private TableLocale _tableLocaleDefault = new TableLocale { };
    public TableLocale TableLocale => CultureInfo.CurrentCulture.Name.StartsWith("fr") ? _tableLocaleFr : _tableLocaleDefault;
}
