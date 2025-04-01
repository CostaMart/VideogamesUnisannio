this game uses OID system (Open items definiition system)

# How to Use
to define an item you cane just do as in the ItemList.json file example
you can reference each "status class" and use its parameters ad an operator to create a new item.
within a status class you can reference any of its field.

target element in the json is the field to which apply the change. 

so let's say we want to apply add 100 to the "health" field (for the sake of this example, let's say health is the field 0) of a CharStat with our item:

our item will look like this:

{
    "items": [
        {
            "id": 0,
            "name": "item 1",
            "effects": [
                {
                    "effectType": "sa",
                    "target": "CharStats.0",
                    "expr": "@CharStats.0 + 100",
                }
            ]
        },
    ]
}

you can define bullets too in the bullets file in the same exact way their effect will be applied on impact if you have the right bullet equipped.

following we will leave a list of the available stat classes and their fields:

- CharStats
     rotationSpeed = 0.5f;
     life = 100;
     aimRotationSpeed = 5f;
     jumpSpeed = 5f;
     moveSpeed = 5f;
     speedLimitBeforeRagdolling = 20f;
     maxJumps = 1;

- PysicalStats
    mass = 1f;
    isaffectedByGravity = true;
    forcey = -1f;
    forcex = -1f;
    forcez = -1f;
    linearDumping = 2f;

- WeaponStats
    baseMagSize;
    baseFireRate;
    magSize;
    fireRate;
    fireStrength = 1f;

- BulletStats
    damage = 1f;
    baseDamage;
    baseWeight;
    explosionRadius;
