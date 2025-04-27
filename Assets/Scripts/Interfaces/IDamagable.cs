using UnityEngine;

// use Interfaces when you need multiple types of contracts
// when you want to inforce behaviours without logic
// other examples: ICollectable, IInteractable,  
// Interfaces are used to define a contract for classes that implement them.inherited object must contain its methods
// objects can inherit from multiple interfaces
// big difference between interfaces and abstract classes is that interfaces cannot have any implementation.
// Interfaces cant have fields / varaibles / reference types - only properties
// 

public interface IDamagable //: NON - MonoBehaviour - Interfaces cannot inherit from base classes eg, MonoBehaviour
{
    // 
    void GetDamage(float damage); // this is the method that will be used to get damage

}
