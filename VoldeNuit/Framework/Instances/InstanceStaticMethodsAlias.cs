namespace VoldeNuit.Framework.Instances;

public abstract partial class Instance {

    public static Instance CreateInstance(float X, float Y, Type objectName, float depth = 0f) {

        return instance_create(X, Y, objectName, depth);
    }

    public static bool IsInstanceExists(Type objectName) {

        return instance_exists(objectName);
    }

    public static bool IsInstanceExists(Instance ID) {
        
        return instance_exists(ID);
    }

    public static bool Destroy(Instance ID, bool eventFlag = true) { 
        
        return instance_destroy(ID, eventFlag);
    }

    public static bool Destroy(Type objectName, bool eventFlag = true) {
        
        return instance_destroy(objectName, eventFlag);
    }

    public static Instance? GetInstanceOnPosition(float X, float Y, Type objectName) {

        return instance_position(X, Y, objectName);
    }

    public static Instance? GetInstanceOnPosition(float X, float Y, Instance ID) {

        return instance_position(X, Y, ID);
    }

    public static List<Instance> GetListOnPosition(float X, float Y, Type objectName) {

        return instance_position_list(X, Y, objectName);
    }

    public static Instance? FindInstance(Type objectName, int index = 0) {

        return instance_find(objectName, index);
    }

    public static Instance? FindInstance(Type ID) {

        return instance_find(ID);
    }

    public static List<Instance> GetListInstance(Type objectName) {

        return instance_find_list(objectName);
    }
}