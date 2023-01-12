using System.Collections.Generic;

public class NFTClasses
{
    #region Respons Classes
    [System.Serializable]
    public class NFTResponse
    {
        public List<Result> result;

        [System.Serializable]
        public class Result
        {
            public int rank;
            public string dishName;
            public int couponId;
            public int bestTime;
            public string userEmail;
            public string status;
            public string redeemedDate;
        }
    }

    [System.Serializable]
    public class CouponResponse
    {
        public Result result;

        [System.Serializable]
        public class Result
        {
            public string restaurantName;
            public int couponId;
            public int amount;
            public int redeemed;
            public string couponUrl;
        }
    }

    [System.Serializable]
    public class WinnersResponse
    {
        public List<Winner> winners;
        public bool tokensMinted;

        [System.Serializable]
        public class Winner
        {
            public string userAddress;
            public string dishName;
            public string userEmail;
            public int bestTime;
            public int couponId;
        }
    }
    #endregion




    #region Body Classes
    public class TopWinnersRequestBody
    {
        public string restaurantName;
    }

    public class WinnindRequestBody
    {
        public int dishId;
        public string userAddress;
        public string userEmail;
        public int time;
    }

    public class RedeemRequestBody
    {
        public string dishName;
        public string userAddress;
    }

    public class WithdrawRequestBody
    {
        public string dishName;
        public string userAddress;
        public string toAddress;
    }
    #endregion
}
