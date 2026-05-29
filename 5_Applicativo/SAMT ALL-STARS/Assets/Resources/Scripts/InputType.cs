namespace Resources.Scripts
{
    // Da dove sta arrivando un input: tastiera o controller.
    // Serve ai PlayerController per filtrare cosa ascoltare (P1 = tastiera, P2 = pad).
    public enum InputType
    {
        Keyboard,
        Controller
    }
}
