public class PeoplePanelController
{
    private PeoplePanelService _peoplePanelService;

    public PeoplePanelController(PeoplePanelService peoplePanelService)
    {
        _peoplePanelService = peoplePanelService;
    }

    public void AcceptCommand(Command<PeoplePanelCommand> command)
    {
    }

    public void MouseOver(bool isOver)
    {
        if (isOver)
        {
            if (_peoplePanelService.GetCurrentPeopleCount() >= _peoplePanelService.GetCurrentPeopleCapacity())
            {
                _peoplePanelService.SwitchPeopleNotEnoughLabel(true);
            }
        }
        else
        {
            _peoplePanelService.SwitchPeopleNotEnoughLabel(false);
        }
    }
}