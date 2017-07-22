# BrowserPortHelper
An utility helper to capture currently open browser tabs and also to open a set of tabs in the desired browser given a list of urls.

Chrome:
-> In chrome we can only access the url of currently active tab. For being able to capture all the open tabs, we trigger a 'ctrl + tab' key press event repetatively to shift the focus to all the tabs inside a window and then read the url.
-> In case the browser window is minimized, no UI will be rendered. This will result in an empty list of tabs when one tries to look up programatically. In order to handle this issue, we first invoke a "Alt + TAB" key press event multiple times to ensure all the open windows are maximized.

Both the "Ctrl + TAB" and "ALT + TAB" approaches are quite clumpsy as they visually keep shifting the focus from one window to the other.
Will stick with this, until I find a more elegant solution for this problem. 
