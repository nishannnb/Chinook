using Chinook.ClientModels;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
	public class PlaylistService : IPlaylist
	{
		private ChinookContext _context;

		public PlaylistService(ChinookContext context)
		{
			_context = context;
		}

		public Playlist GetPlaylistByPlaylistId(long playlistId, string userId)
		{
			Playlist playlist = _context.Playlists
		   .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
		   .Where(p => p.PlaylistId == playlistId)
		   .Select(p => new Playlist()
		   {
			   Name = p.Name,
			   Tracks = p.Tracks.Select(t => new PlaylistTrack()
			   {
				   AlbumTitle = t.Album.Title,
				   ArtistName = t.Album.Artist.Name,
				   TrackId = t.TrackId,
				   TrackName = t.Name,
				   IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites")).Any()
			   }).ToList()
		   })
		   .FirstOrDefault();

			return playlist;
		}
	}
}
