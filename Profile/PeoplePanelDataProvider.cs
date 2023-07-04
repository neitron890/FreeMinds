using System.Threading;
using Storage;
using UnityEngine;

public class PeoplePanelDataProvider
{
    private readonly IStorage _storage;

    public PeoplePanelDataProvider(IStorage storage)
    {
        _storage = storage;
    }

    public Transform PeopleNotEnoughPanel =>
        _storage.Get<Transform>(
            "Canvas/" +
            "ProfilePanel(Clone)/NotEnoughHousesLabel(Clone)").Result;

    public int CurrentPeopleCount => (int) _storage.UserData.Value.PeopleCount.Value;

    public int CurrentPeopleCapacity => _storage.UserData.Value.PopulationLimit.Value;
}