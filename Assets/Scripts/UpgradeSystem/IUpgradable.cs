interface IUpgradable
{
    int ID { get; }
    void GetId()
    {
        // qui si recuperà l'ID da file
    }
    void Upgrade(Upgrade upgrade);
}