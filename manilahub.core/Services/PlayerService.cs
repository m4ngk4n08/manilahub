using manilahub.core.Services.IServices;
using manilahub.data.Entity;
using manilahub.data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace manilahub.core.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public Player Get(string username)
        {
            return _playerRepository.Get(username);
        }

        public IEnumerable<Player> GetPendingStatus()
        {
            return _playerRepository.GetAll().Where(j => j.Status is 1);
        }

        public Player UpdateStatus(Player model)
        {
            return _playerRepository.UpdatePlayerStatus(model);
        }
    }
}
