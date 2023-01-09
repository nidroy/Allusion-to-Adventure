using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    [UnityTest]
    public IEnumerator TestTree()
    {
        Wood wood = new Wood();

        yield return new WaitForSeconds(0.1f);

        Assert.False(wood.isStump);
    }

    [UnityTest]
    public IEnumerator TestPatrolling()
    {
        Character character = new Character();
        Characteristics characteristics = new Characteristics();

        character.characteristics = characteristics;

        characteristics.UpdateTypeOfMoving(character);

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("Patrolling", characteristics.typeOfMoving);
    }

    [UnityTest]
    public IEnumerator TestDurability()
    {
        Character character = new Character();
        Equipment equipment = new Equipment();
        Item item = new Item();
        ItemData itemData = new ItemData();

        itemData.durability = 2;
        item.data = itemData;
        equipment.weapon = item;
        character.equipment = equipment;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(2, character.equipment.weapon.data.durability);
    }
}
