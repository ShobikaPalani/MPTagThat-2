﻿#region Copyright (C) 2020 Team MediaPortal
// Copyright (C) 2020 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.
#endregion

#region

using System.Linq;
using MPTagThat.Core.Common.Song;
using Raven.Client.Documents.Indexes;

#endregion

namespace MPTagThat.Core.Services.MusicDatabase.Indexes
{
  /// <summary>
  /// Map Reduce Index to retrieve distinct AlbumArtists
  /// </summary>
  public class DistinctAlbumArtistIndex : AbstractIndexCreationTask<SongData, DistinctResult>
  {
    public DistinctAlbumArtistIndex()
    {
      Map = tracks => from track in tracks
                            from albumartists in track.AlbumArtist.Split(';').ToList()
                            select new { Name = albumartists };
      
      Reduce = results => from result in results
                          group result by result.Name into g
                          select new { Name = g.Key };

      Store(song => song.Name, FieldStorage.Yes);
    }
  }
}