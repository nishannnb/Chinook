using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
	public class ArtistService : IArtist
	{
		private ChinookContext _context;

		public ArtistService(ChinookContext context)
		{
			_context = context;
		}

		public List<Artist> GetArtistsByArtistName(string artistName)
		{
			return _context.Artists.Where(c => c.Name == artistName).ToList();
		}

		public async Task<List<Artist>> GetArtists()
		{
			return await _context.Artists.ToListAsync();
		}

		public async Task<List<Album>> GetAlbumsByArtistId(int artistId)
		{
			return await _context.Albums.Where(a => a.ArtistId == artistId).ToListAsync();
		}

		public Artist GetArtistByArtistId(long artistId, string userId)
		{
			Artist artist = _context.Artists.SingleOrDefault(a => a.ArtistId == artistId);
			return artist;
		}

		public List<PlaylistTrack> GetPlaylistTrackByArtistId(long artistId, string userId)
		{
			List<PlaylistTrack> playlistTracks = _context.Tracks.Where(a => a.Album.ArtistId == artistId)
				.Include(a => a.Album)
				.Select(t => new PlaylistTrack()
				{
					AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
					TrackId = t.TrackId,
					TrackName = t.Name,
					IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "My favorite tracks")).Any()
				})
				.ToList();

			return playlistTracks;
		}

		public bool CreateMyFavoritePlayList(string userId)
		{
			var filter = _context.UserPlaylists.Any(a => a.UserId == userId && a.Playlist.Name.ToLower() == "My favorite tracks".ToLower());
			if (filter)
				return false;

			var playlist = _context.Playlists.SingleOrDefault(c => c.Name == "My favorite tracks");
			if (playlist == null)
				return false;

			_context.UserPlaylists.Add(new UserPlaylist { PlaylistId = playlist.PlaylistId, UserId = userId });
			_context.SaveChanges();

			return true;
		}
	}
}
