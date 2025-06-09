namespace Poker.Enumerations
{
    public enum Position
    {
        None, // Penambahan posisi None untuk menghindari null reference
        Dealer,
        Small_Blind,
        Big_Blind,
        Early_Position,
        Middle_Position,
        Late_Position
    }
}