﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace Microsoft.CodeAnalysis.Editor.Implementation.InlineRename
{
    internal partial class RenameCommandHandler : VSCommanding.ICommandHandler<ReturnKeyCommandArgs>
    {
        public VSCommanding.CommandState GetCommandState(ReturnKeyCommandArgs args)
        {
            return GetCommandState();
        }

        public bool ExecuteCommand(ReturnKeyCommandArgs args, CommandExecutionContext context)
        {
            if (_renameService.ActiveSession != null)
            {
                _renameService.ActiveSession.Commit();
                //(args.TextView as IWpfTextView).VisualElement.Focus();
                return true;
            }

            return false;
        }
    }
}
