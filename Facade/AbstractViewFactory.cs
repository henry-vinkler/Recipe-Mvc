using RecipeMvc.Aids;
using RecipeMvc.Data;

namespace RecipeMvc.Facade;
public abstract class AbstractViewFactory<TData, TView> 
    where TData: EntityData, new() 
    where TView: EntityView, new() {
    public virtual TView CreateView(TData? d){
        var v = new TView();
        if (d is not null) Copy.Members(d, v);
        return v;
    }
    public virtual TData CreateData(TView? v){
        var d = new TData();
        if (v is not null) Copy.Members(v, d);
        return d;
    }
}
