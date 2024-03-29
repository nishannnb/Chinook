using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services
{
	public interface IArtist
	{
		Task<List<Artist>> GetArtists();

		List<Artist> GetArtistsByArtistName(string artistName);

		Task<List<Album>> GetAlbumsByArtistId(int artistId);

		Artist GetArtistByArtistId(long artistId, string userId);

		List<PlaylistTrack> GetPlaylistTrackByArtistId(long artistId, string userId);

		bool CreateMyFavoritePlayList(string userId);
	}
}
