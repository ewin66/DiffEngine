using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

static class MenuBuilder
{
    public static ContextMenuStrip Build(Action exit, Tracker tracker)
    {
        var menu = new ContextMenuStrip();
        var items = menu.Items;

        menu.Opening += delegate
        {
            CleanTransientMenus(items);
            foreach (var item in BuildTrackingMenuItems(tracker))
            {
                items.Insert(0, item);
            }
        };
        menu.Closed += delegate { CleanTransientMenus(items); };

        var exitItem = new MenuButton("Exit", exit, Images.Exit);
        items.Add(exitItem);

        return menu;
    }

    static void CleanTransientMenus(ToolStripItemCollection items)
    {
        var toRemove = items.Cast<ToolStripItem>()
            .Where(x => x.Text != "Exit");
        items.RemoveRange(toRemove);
    }

    static IEnumerable<ToolStripItem> BuildTrackingMenuItems(Tracker tracker)
    {
        if (!tracker.TrackingAny)
        {
            yield break;
        }

        yield return new ToolStripSeparator();
        yield return new MenuButton("Accept All", tracker.AcceptAll, Images.AcceptAll);
        yield return new MenuButton("Clear", tracker.Clear, Images.Clear);

        if (tracker.Deletes.Any())
        {
            yield return new ToolStripSeparator();
            foreach (var delete in tracker.Deletes)
            {
                yield return new MenuButton($"{delete.Name}", () => tracker.Accept(delete));
            }

            yield return new MenuButton("Pending Deletes:", tracker.AcceptAllDeletes, Images.Delete);
        }

        if (tracker.Moves.Any())
        {
            yield return new ToolStripSeparator();
            foreach (var move in tracker.Moves)
            {
                yield return new MenuButton($"{move.Name} ({move.Extension})", () => tracker.Accept(move));
            }

            yield return new MenuButton("Pending Moves:", tracker.AcceptAllMoves, Images.Accept);
        }
    }
}