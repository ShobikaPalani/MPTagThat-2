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

# region

using CommonServiceLocator;
using MPTagThat.Core.Common.Song;
using MPTagThat.Core.Services.Logging;
using MPTagThat.Core.Services.Settings;
using MPTagThat.Core.Services.Settings.Setting;
using System;

#endregion

namespace MPTagThat.SongGrid.Commands
{
  public abstract class Command : IDisposable
  {
    #region Variables

    public static NLogLogger log;
    public static Options options;

    #endregion

    #region Public Properties

    public bool NeedsPreprocessing { get; set; }
    public bool NeedsCallback { get; set; }

    #endregion

    /// <summary>
    /// Execute the Command
    /// </summary>
    /// <param name="song"></param>
    /// <returns></returns>
    public virtual bool Execute(SongData song)
    {
      return false;
    }

    /// <summary>
    ///  Do Preprocessing of the Tracks
    /// </summary>
    /// <param name="song"></param>
    /// <returns></returns>
    public virtual bool PreProcess(SongData song)
    {
      return false;
    }

    /// <summary>
    /// Post Process after command execution
    /// </summary>
    /// <returns></returns>
    public virtual bool PostProcess()
    {
      return false;
    }

    /// <summary>
    /// Create a Command Object
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static Command Create(object[] parameters)
    {
      if (parameters == null || parameters.GetLength(0) == 0)
      {
        throw new Exception("Command must not be empty");
      }

      var command = (string)parameters[0];

      if (!CommandTypes.AvailableCommands.ContainsKey(command))
      {
        throw new Exception(string.Format("Command not supported: {0}", command));
      }

      Type commandType = CommandTypes.AvailableCommands[command];
      log = (ServiceLocator.Current.GetInstance(typeof(ILogger)) as ILogger).GetLogger;
      options = (ServiceLocator.Current.GetInstance(typeof(ISettingsManager)) as ISettingsManager).GetOptions;

      try
      {
        Command commandobj = (Command)Activator.CreateInstance(commandType, new object[] { parameters });
        return commandobj;
      }
      catch (System.Reflection.TargetInvocationException e)
      {
        throw e.InnerException;
      }
    }

    /// <summary>
    /// Indicator, that Command processing got interupted by user
    /// </summary>
    public bool ProgressCancelled { get; set; }

    public void Dispose()
    {

    }
  }
}