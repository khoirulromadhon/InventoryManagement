using InventoryManagement.ViewModels;

namespace InventoryManagement.Service.Interface
{
    public interface IGoodMutationService
    {
        Task Mutation(VmGoodMutation vmGoodMutation);
        Task<List<VmGoodMutation>> MutationHistory(string keyword, int cursor);
    }
}
