using System;
using UnityEngine;

public class FuncPredicate : IPredicate
{
    private Func<bool> _func;
    
    public FuncPredicate(Func<bool> func) => _func = func;
    
    public bool Evaluate() => _func.Invoke();
}
