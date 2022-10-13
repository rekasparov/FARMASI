using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Common.BaseSettings.RabbitMQ
{
    public static class RabbitMQQueueNames
    {
        public static string DecreaseStockRequestMessageQueueName { get => "decrease-stock-request"; }
        public static string StockNotAvailableResponseMessageQueueName { get => "stock-not-available-response"; }
        public static string AddToCartRequestMessageQueueName { get => "add-to-cart-request"; }
        public static string AddedToCartResponseMessageQueueName { get => "added-to-cart-response"; }
        public static string DeleteFromCartRequestMessageQueueName { get => "delete-from-cart-request"; }
        public static string DeletedFromCartResponseMessageQueueName { get => "deleted-from-cart-response"; }

    }
}
