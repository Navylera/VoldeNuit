namespace VoldeNuit.Framework.Instances;

public abstract partial class Instance {

    public void DrawSelf() {
        
        draw_self();
    }

    public Instance? GetInstanceMeetsWithAt(float X, float Y, Type objectName) {
        
        return instance_place(X, Y, objectName);
    }

    public Instance? GetInstanceMeetsWithAt(float X, float Y, Instance ID) {

        return instance_place(X, Y, ID);
    }

    public List<Instance> GetListMeetsWithAt(float X, float Y, Type objectName) {
        
        return instance_place_list(X, Y, objectName);
    }

    public void Activate() {
        
        activate();
    }

    public void Deactivate() {
        
        deactivate();
    }
}