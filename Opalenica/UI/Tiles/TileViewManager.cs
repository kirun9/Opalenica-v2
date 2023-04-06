namespace Opalenica.UI.Tiles;

public class TileViewManager
{
    private static Dictionary<string, TileView> registeredViews = new Dictionary<string, TileView>();
    public static void RegisterView(TileView view)
    {
        if (registeredViews.ContainsKey(view.ViewID))
            throw new DuplicateTileViewException(view.ViewID);
        else
            registeredViews.Add(view.ViewID, view);
    }

    public static TileView GetTileView(string viewID)
    {
        if (registeredViews.ContainsKey(viewID))
            return registeredViews[viewID];
        else
            throw new TileViewNotFoundException(viewID);
    }
}
