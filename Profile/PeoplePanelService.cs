
public class PeoplePanelService
{
    private PeoplePanelDataProvider _panelDataProvider;
    
    public PeoplePanelService(PeoplePanelDataProvider panelDataProvider)
    {
        _panelDataProvider = panelDataProvider;
    }

    public int GetCurrentPeopleCount()
    {
        return _panelDataProvider.CurrentPeopleCount;
    }
    
    public int GetCurrentPeopleCapacity()
    {
        return _panelDataProvider.CurrentPeopleCapacity;
    }

    public void SwitchPeopleNotEnoughLabel(bool enable)
    {
        _panelDataProvider.PeopleNotEnoughPanel.gameObject.SetActive(enable);
    }
}
