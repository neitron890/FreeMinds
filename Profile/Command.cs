using System;

public struct Command<TCommandType>
{
    public TCommandType CommandType;
    public object Data;
    public Action<bool> Callback;
}
