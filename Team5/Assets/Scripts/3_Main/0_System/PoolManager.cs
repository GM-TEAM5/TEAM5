using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BW.Util;
using UnityEditor;
using UnityEngine;

// using Redcode.Pools;

[Serializable]
internal struct PoolData
{
    [SerializeField]
    private string _name;

    public string Name => _name;

    [SerializeField]
    private Component _component;

    public Component Component => _component;

    [SerializeField]
    [Min(0)]
    private int _count;

    public int Count => _count;

    [SerializeField]
    private Transform _container;

    public Transform Container => _container;

    [SerializeField]
    private bool _nonLazy;

    public bool NonLazy => _nonLazy;
}

/// <summary>
/// Pool manager. You can set options for it in editor and then use in game. <br/>
/// It creates specified pools in Awake method, which then you can find with <b>GetPool</b> methods and call its methods.
/// </summary>
public class PoolManager : Singleton<PoolManager>

{
    public int aliveEnemiesNum;
    public bool enemyNotExists => aliveEnemiesNum <= 0;
    
    
    [SerializeField]
    private List<PoolData> _pools;

    private readonly List<IPool<Component>> _poolsObjects = new();

    private void Start()
    {
        Init();
    }



    public void Init()
    {
        var namesGroups = _pools.Select(p => p.Name).GroupBy(n => n).Where(g => g.Count() > 1);

        if (namesGroups.Count() > 0)
            throw new Exception($"Pool Manager already contains pool with name \"{namesGroups.First().Select(g => g).First()}\"");

        var poolsType = typeof(List<IPool<Component>>);
        var poolsAddMethod = poolsType.GetMethod("Add");
        var genericPoolType = typeof(Pool<>);

        foreach (var poolData in _pools)
        {
            var poolType = genericPoolType.MakeGenericType(poolData.Component.GetType());
            var createMethod = poolType.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic);

            var pool = createMethod.Invoke(null, new object[] { poolData.Component, poolData.Count, poolData.Container });

            if (poolData.NonLazy)
            {
                var nonLazyMethod = poolType.GetMethod("NonLazy");
                nonLazyMethod.Invoke(pool, null);
            }

            poolsAddMethod.Invoke(_poolsObjects, new object[] { pool });
        }
    }

    #region Get pool
    /// <summary>
    /// Find pool by <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="T">Pool's objects type.</typeparam>
    /// <param name="index">Pool index.</param>
    /// <returns>Finded pool.</returns>
    public IPool<T> GetPool<T>(int index) where T : Component => (IPool<T>)_poolsObjects[index];

    /// <summary>
    /// Find pool by type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Pool's objects type.</typeparam>
    /// <returns>Finded pool.</returns>
    public IPool<T> GetPool<T>() where T : Component => (IPool<T>)_poolsObjects.Find(p => p.Source is T);

    /// <summary>
    /// Find pool by <paramref name="name"/>
    /// </summary>
    /// <typeparam name="T">Pool's objects type.</typeparam>
    /// <param name="name">Pool name.</param>
    /// <returns>Finded pool.</returns>
    public IPool<T> GetPool<T>(string name) where T : Component => (IPool<T>)_poolsObjects[_pools.FindIndex(p => p.Name == name)];
    #endregion

    #region Get from pool
    /// <summary>
    /// Find pool by <paramref name="index"/> and gets object from it.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="GetPool{T}"/></typeparam>
    /// <param name="index"><inheritdoc cref="GetPool{T}"/></param>
    /// <returns>Pool's object (or <see langword="null"/> if free object was not finded).</returns>
    public T GetFromPool<T>(int index) where T : Component => GetPool<T>(index).Get();

    /// <summary>
    /// Find pool by type <typeparamref name="T"/> and gets object from it.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="GetPool{T}"/></typeparam>
    /// <returns>Pool's object (or <see langword="null"/> if free object was not finded).</returns>
    public T GetFromPool<T>() where T : Component => GetPool<T>().Get();

    /// <summary>
    /// Find pool by name <paramref name="name"/> and gets object from it.
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="GetPool{T}"/></typeparam>
    /// <param name="name"><inheritdoc cref="GetPool{T}(string)"/></param>
    /// <returns>Pool's object (or <see langword="null"/> if free object was not finded).</returns>
    public T GetFromPool<T>(string name) where T : Component => GetPool<T>(name).Get();
    #endregion

    #region Take to pool
    /// <summary>
    /// Returns object back to pool and marks it as free.
    /// </summary>
    /// <param name="index"><inheritdoc cref="GetPool{T}"/></param>
    /// <param name="component">Object (its component) which returns back.</param>
    public void TakeToPool(int index, Component component) => _poolsObjects[index].Take(component);

    /// <summary>
    /// Returns object back to pool and marks it as free.
    /// </summary>
    /// <typeparam name="T">Pool type.</typeparam>
    /// <param name="component">Object (its component) which returns back.</param>
    public void TakeToPool<T>(Component component) where T : Component => GetPool<T>().Take(component);

    /// <summary>
    /// Returns object back to pool and marks it as free.
    /// </summary>
    /// <typeparam name="T">Pool type.</typeparam>
    /// <param name="name"><inheritdoc cref="GetPool{T}(string)"/></param>
    /// <param name="component">Object (its component) which returns back.</param>
    public void TakeToPool<T>(string name, Component component) where T : Component => GetPool<T>(name).Take(component);
    #endregion



    //================================================================================
    public EnemySpawner GetEnemySpawner(string id, Vector3 initPos)
    {
        aliveEnemiesNum++; 
        
        EnemySpawner enemySpawner = GetFromPool<EnemySpawner>(); 
        enemySpawner.SpawnEnemy( id, initPos, 2f);
    
        return enemySpawner;
    }


    public Enemy GetEnemy( string id ,Vector3 initPos)
    {
        Enemy enemy = GetFromPool<Enemy>(); 
        EnemyDataSO enemyData =  ResourceManager.Instance.GetEnemyData(id);
        enemy.Init( enemyData ,initPos);
    
        
    
        return enemy;
    }

    public void TakeEnemy(Enemy e)
    {
        aliveEnemiesNum--;

        TakeToPool<Enemy>(e);
    }



    public EnemyProjectile GetEnemyProjectile(EnemyAbilitySO abilityData, Enemy enemy, Vector3 initPos, float lifeTime)
    {
        EnemyProjectile enemyProj = GetFromPool<EnemyProjectile>(); 
        enemyProj.Init(abilityData, enemy, initPos, lifeTime);
    
        return enemyProj;
    }


    public DamageText GetDamageText(Vector3 hitPoint, float damage, DamageType type)
    {
        DamageText damageText = GetFromPool<DamageText>();
        damageText.Init(hitPoint, damage, type);
        return damageText;
    }

    
    public DamageText GetText(Vector3 position, string content)
    {
        DamageText damageText = GetFromPool<DamageText>();
        damageText.Init(position, content);
        return damageText;
    }
    
    
    

    public DropItem GetExp(float value, Vector3 initPos)
    {
        DropItem dropItem = GetFromPool<DropItem>();
        DropItemDataSO itemData =  ResourceManager.Instance.GetDropItemData("00");
        dropItem.Init(itemData,value,initPos);
        return dropItem;
    }

    public DropItem GetHpUp(float value, Vector3 initPos)
    {
        DropItem dropItem = GetFromPool<DropItem>();
        DropItemDataSO itemData =  ResourceManager.Instance.GetDropItemData("01");
        dropItem.Init(itemData,value,initPos);
        return dropItem;
    }


    public DropItem GetMoney(float value, Vector3 initPos)
    {
        DropItem dropItem = GetFromPool<DropItem>();
        DropItemDataSO itemData =  ResourceManager.Instance.GetDropItemData("02");
        dropItem.Init(itemData,value,initPos);
        return dropItem;
    }

    public DropItem GetInk(float value, Vector3 initPos)
    {
        DropItem dropItem = GetFromPool<DropItem>();
        DropItemDataSO itemData =  ResourceManager.Instance.GetDropItemData("03");
        dropItem.Init(itemData,value,initPos);
        return dropItem;
    }
    
    public AreaIndicator GetAreaIndicator_Circle(Vector3 initPos,Vector3 targetPos, Vector2 size, float duration)
    {
        AreaIndicator areaIndicator = GetFromPool<AreaIndicator>();
        AreaIndicatorSO data = (AreaIndicatorSO)ResourceManager.Instance.areaIndicatorData.GetData("00");
        areaIndicator.Init(data,initPos, targetPos, size, duration );

        return areaIndicator;
    }

    public AreaIndicator GetAreaIndicator_RectDir(Vector3 initPos, Vector3 targetPos, Vector2 size, float duration)
    {
        AreaIndicator areaIndicator = GetFromPool<AreaIndicator>();
        AreaIndicatorSO data = (AreaIndicatorSO)ResourceManager.Instance.areaIndicatorData.GetData("01");
        areaIndicator.Init(data,initPos, targetPos, size, duration );

        return areaIndicator;
    }


    //=====================================================================
    

}
