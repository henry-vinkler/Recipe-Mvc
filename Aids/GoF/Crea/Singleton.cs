namespace RecipeMvc.Aids.GoF.Crea;

public class Singleton { 
    private readonly static Singleton instance = new ();
    private Singleton() { }
    public static Singleton New() => instance;
}





