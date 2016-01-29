namespace Game
{
    public enum Type_Platform
    {
        Classic,
        Bouncy,
        Hight
    }


    public enum Type_Enemy
    {
        CrazyFireball,
        Shooter,
        Meteor,
        Tnt
    }

    public enum Type_Bonus
    {
        Note,
        Invincibility,
        Heart,
        Power
    }

    public enum Type_Bullet
    {
        Enemy,
        Player,
        Special
    }

    public enum Type_HUD
    {
        Life,
        Stamina,
        Special,
        Jinjo_Violet_On,
        Jinjo_Violet_Off,
        Jinjo_Yellow_On,
        Jinjo_Yellow_Off,
        Jinjo_Orange_On,
        Jinjo_Orange_Off,
        Jinjo_Green_On,
        Jinjo_Green_Off,
        Jinjo_Blue_On,
        Jinjo_Blue_Off,
        Jinjo_Black_On,
        Jinjo_Black_Off,
        GameOver,
        Cooldown,
        Pause
    }

    public enum Audio_Type
    {
        Music,
        Rewind,
        Jinjo,
        AllJinjos,
        Kamehameha,
        Shield,
        ExplosionTNT,
        Note,
        Tornado,
        Countdown,
        Go
    }

    public class Data
    {
        public static bool ACCESSIBILITY_MODE = false;
        public static int CURRENT_SCORE = 0;
    }
}
