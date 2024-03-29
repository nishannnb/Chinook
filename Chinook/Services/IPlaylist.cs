using Chinook.ClientModels;

namespace Chinook.Services
{
	public interface IPlaylist
	{
		Playlist GetPlaylistByPlaylistId(long playlistId, string userId);
	}
}
