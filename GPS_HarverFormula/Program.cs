using HaversineFormula;


namespace Program
{
    class Program
    {
        
        static void Main(string[] args)
        {

            int i = 0;  //좌표 받을 갯수 :: 0부터
            double alldist = 0;
            double avrdist = 0;

            double firstcood_lat;   //기준좌표 위도 저장 할 변수
            double firstcood_long;  //기준좌표 위도 저장 할 변수

            double seccood_lat;
            double seccood_long;

            Position pos1 = new Position();     //기준 좌표 생성자
            Position pos2 = new Position();     //비교 좌표 생성자

            Haversine hv = new Haversine();     //haversie class 생성자

            //위도 : 35도04분42.21초
            //경도 : 129도 04분 44.26초
            //표고 : 6.4 m
            //35 + (4/60)+(42.21/3600)
            //국토정보플랫폼 참고

            firstcood_lat = 35 + (4/60)+(42.21/3600);  //국토정보플랫폼을 기반으로 도출 된 위도
            firstcood_long = 129+(4/60)+(44.26/3600); //국토정보플랫폼을 기반으로 도출 된 경도
            //Console.WriteLine("기준 좌표값");

            Console.WriteLine($"위도 -> {firstcood_lat}");
            Console.WriteLine($"경도 -> {firstcood_long}");

            pos1.Latitude = firstcood_lat;      //기준좌표계의 위도 저장
            pos1.Longitude = firstcood_long;    //기준좌표계의 경도 저장

            while (true)
            {

                Console.WriteLine("=======================");

                Console.Write("위도 ->");
                seccood_lat = double.Parse(Console.ReadLine()); 
                Console.Write("경도 ->");
                seccood_long = double.Parse(Console.ReadLine());
                

                pos2.Latitude = seccood_lat;    //GPS에서 도출된 위도값
                pos2.Longitude = seccood_long;  //GPS에서 도출된 경도값

                //DistanceType : Miles, Kilometers
                double result = hv.Distance(pos1, pos2, DistanceType.Kilometers);   //저장한 기준 좌표, 측정좌표 거리계산 함수로 넘겨준 후, 계산된 거리 값 저장
                Console.WriteLine($"거리 :  {result}(m)");    
                alldist = alldist + result;     //계산된 거리값 누적 합산하기
                Console.WriteLine("\n");

                i++;    //거리 계산 완료 후 1 증가. 총 10개의 값으로 계산

                if (i == 10)    //10번 카운터가 완료 되었으면,
                {
                    avrdist = alldist / 10;                             //거리 평균값 계산
                    Console.WriteLine($"거리 평균값 : {avrdist} (m)");   //거리 평균 계산값 출력

                    break;  //프로그램 종료

                }

            }        
            
        }


    }

}

namespace HaversineFormula
{
    /// <summary>
    /// The distance type to return the results in.
    /// 제일 정확함
    /// </summary>

    public enum DistanceType { Miles, Kilometers };

    /// <summary>
    /// Specifies a Latitude / Longitude point.
    /// </summary>
    public struct Position
    {
        public double Latitude;     //위도
        public double Longitude;    //경도
    }

    class Haversine
    {
        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name=”pos1″></param>
        /// <param name=”pos2″></param>
        /// <param name=”type”></param>
        /// <returns></returns>

        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            
            double R = (type == DistanceType.Miles) ? 3960 : 6371;//지구 반경 약 3960 miles,약 6371 km

            //기준점 및 측정 gps 거리 계산
            double dLat = this.toRadian(pos2.Latitude - pos1.Latitude);
            double dLon = this.toRadian(pos2.Longitude - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(pos1.Latitude)) * Math.Cos(this.toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            
            return d*1000 ; //km 를 m로 환산
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double toRadian(double val)
        {
            //Degree to Radian
            return (Math.PI / 180) * val;
        }
    }
}