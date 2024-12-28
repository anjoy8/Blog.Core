namespace Blog.Core.Model.ViewModels.RootTKey
{
    public class SysUserInfoDtoRoot<Tkey> where Tkey : IEquatable<Tkey>
    {
        public Tkey uID { get; set; }

        public List<Tkey> RIDs { get; set; }

    }
}
