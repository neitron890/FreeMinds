namespace Profile
{
	public interface IProfileService
	{
		void ShowProfileWindow();
		void Logout();
		void SetProfile();
		void ChangeName(string newName);
		void Close();
	}
}