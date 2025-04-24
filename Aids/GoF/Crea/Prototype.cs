namespace RecipeMvc.Aids.GoF.Crea;

public interface ICloneable<T> where T:class  { 
    T Clone(); 
}
public class Prototype { 
    public T Clone<T>(T x) where T: class, ICloneable<T> => x.Clone() ; 
}


