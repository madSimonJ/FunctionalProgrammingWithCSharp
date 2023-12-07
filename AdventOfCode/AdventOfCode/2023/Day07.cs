namespace AdventOfCode._2023
{
    public record Hand
    {
        public string Cards { get; set; }
        public int Wager { get; set; }
    }

    public enum HandType
    {
        FiveOfKind,
        FourOfKind,
        FullHouse,
        ThreeOfKind,
        TwoPair,
        OnePair,
        HighestCard
    }

    public record HandWithScoreType : Hand
    {
        public HandType Type { get; set; }
    }

    public record HandWithScoreTypeAndRank : HandWithScoreType
    {
        public int Rank { get; set; }
    }

    public class HandTypeComparer : IComparer<HandType>
    {
        public int Compare(HandType x, HandType y) =>
            x > y
                ? -1
                : y > x
                    ? 1
                    : 0;
    }

    public class HandCardValueComparer : IComparer<HandWithScoreType>
    {
        private readonly bool _jokers;

        public HandCardValueComparer(bool jokers = false)
        {
            _jokers = jokers;
        }

        private int calculateRelativeCardValue(char c) =>
            int.TryParse(c.ToString(), out var cP)
                ? cP
                : c switch
                {
                    'T' => 10,
                    'J' => _jokers ? 0 : 11,
                    'Q' => 12,
                    'K' => 13,
                    'A' => 14,
                };

        public int CompareOne(char x, char y) =>
            calculateRelativeCardValue(x) > calculateRelativeCardValue(y)
                ? 1
                : calculateRelativeCardValue(y) > calculateRelativeCardValue(x)
                    ? -1
                    : 0;
        public int Compare(HandWithScoreType? x, HandWithScoreType? y)
        {
            var cardsToCompare = x.Cards.Zip(y.Cards).FirstOrDefault(a => a.First != a.Second);
            return cardsToCompare == default
                ? 0
                : CompareOne(cardsToCompare.First, cardsToCompare.Second);
        }
    }

    public class Day07
    {
        private static readonly HandTypeComparer HandTypeComparer = new HandTypeComparer();
        private static HandCardValueComparer handCardValueComparer;

        private const string RealInput = "72772 82\r\n8Q278 230\r\nQQJQQ 42\r\n77778 148\r\nQAJ8A 528\r\n87A6K 976\r\nTTTT5 957\r\nQJ4QA 704\r\n6K688 432\r\n93A4Q 621\r\nA66J9 120\r\nJ7773 559\r\n88Q88 196\r\nTTT48 320\r\n88887 995\r\n27227 897\r\nJQQ54 99\r\n67666 646\r\n5AT77 537\r\nA96AA 215\r\nAATA8 211\r\nAQQ7J 306\r\nJQ47Q 499\r\n5755J 11\r\nKKKJ5 814\r\nA4527 479\r\nJ7744 26\r\n5ATK7 945\r\nK8Q6J 700\r\n67677 972\r\nKJKKJ 455\r\n9A5J6 338\r\n77Q7Q 421\r\n37762 214\r\nQ572K 561\r\nTKTTT 868\r\n75577 550\r\n99666 584\r\n84AK5 283\r\n34Q49 337\r\nAA4AA 575\r\n8K88J 505\r\n44554 546\r\n6JQQT 757\r\nA5Q25 157\r\n23K23 278\r\n28424 758\r\n44QQ4 183\r\n3TTTT 574\r\n5KJ29 824\r\n2A222 971\r\n557TK 460\r\n8TTTJ 706\r\n38T84 877\r\n282J6 340\r\n6A299 940\r\n73JQ7 573\r\nTTT33 133\r\nK6AQT 297\r\n34782 145\r\n2Q4A3 653\r\n884KJ 533\r\n6Q2J3 689\r\n9TQ99 94\r\nJJ6Q6 693\r\n9999K 397\r\nKA896 734\r\n4T444 962\r\nK4T95 859\r\n6T26J 627\r\nJJJ8J 933\r\n555Q5 5\r\n22232 770\r\n2A22J 424\r\n4448Q 222\r\n46436 776\r\n2J2T2 403\r\n8QJ8Q 393\r\nKT39T 792\r\nKQK65 652\r\nT2292 312\r\nK3TAJ 886\r\n259A4 110\r\n88999 585\r\n5T673 1\r\nJ29K7 45\r\nKKJKK 852\r\n7AJ8Q 996\r\n24J8J 605\r\n7T667 748\r\n8639J 269\r\n25TA9 755\r\nKQQJQ 256\r\nJ6499 97\r\n6T48A 472\r\n95AQ8 902\r\n97979 319\r\n5A555 238\r\n37A7A 872\r\nQ245T 720\r\n37777 275\r\n35533 989\r\nK4AK4 440\r\nT2J48 181\r\n3K857 205\r\n47K65 991\r\n3J383 975\r\n38383 102\r\n6T6TT 982\r\nQAQQ9 33\r\nKK2JK 400\r\nQ6Q6A 143\r\n64836 336\r\nA7A88 241\r\nA9446 348\r\n22252 225\r\n8T944 362\r\n4Q8K2 318\r\n939T9 394\r\n44447 583\r\n334K4 464\r\n2A797 73\r\nKJQQ5 217\r\n77TJ3 285\r\nAAA3A 407\r\n4A2K8 644\r\nTT955 778\r\nQQ8Q8 175\r\n77JKT 107\r\n33833 65\r\nJA8A8 219\r\n867JA 413\r\n4494K 475\r\n42722 208\r\n25535 30\r\nQ282T 988\r\nTTJA3 170\r\n593TA 889\r\n44K44 127\r\nTJJ36 509\r\nQ343A 843\r\n49J53 784\r\nT33T8 106\r\nJ799A 270\r\n66956 862\r\n75465 659\r\n55567 613\r\nQ89JA 361\r\nKK548 529\r\nAQAAA 482\r\n658K2 620\r\n7658A 409\r\n66636 216\r\n23846 961\r\nJ4JJ4 150\r\n26Q66 369\r\nKKKKA 832\r\n5K55K 23\r\n784QA 750\r\n5A7Q4 863\r\nKQKQJ 368\r\n9JKQA 309\r\n3A6K7 954\r\n2439Q 707\r\n222Q4 41\r\n55255 549\r\n22372 900\r\nA65TJ 60\r\nJA77A 667\r\nQQQQK 10\r\n89466 548\r\n966A6 15\r\n884J8 177\r\n3AKK9 195\r\n5AQQQ 738\r\nJ2696 570\r\nAA359 617\r\n5AJ34 678\r\n72222 837\r\n6K325 596\r\nAJAA2 929\r\nTT336 261\r\n7KTTK 990\r\n5A5J5 675\r\nK87J7 501\r\n9QQ93 334\r\nK4747 20\r\n2Q29Q 305\r\nQJ777 47\r\n47478 869\r\n99T99 759\r\n53385 714\r\nQQ5J5 67\r\n6TK44 209\r\n5QA4A 308\r\n82299 568\r\n428QJ 630\r\n44T6T 386\r\nQJJ9T 190\r\n3737K 69\r\n8A738 339\r\n734A9 579\r\n4949T 540\r\nQJ8TT 492\r\nK5AJA 880\r\n2J928 632\r\n24478 924\r\nKQKQK 891\r\n892T8 78\r\n27QK7 396\r\n6JTJ8 939\r\nJJJJJ 542\r\nA2433 375\r\nK38J3 687\r\nTQJQ8 228\r\n64K6T 875\r\n5Q55Q 342\r\n23A49 699\r\nA97J6 522\r\n7TTT4 454\r\nK7J88 805\r\nJ3373 168\r\nQ54T6 331\r\n5Q49Q 728\r\n8ATA4 459\r\nTT685 191\r\n9887J 745\r\n24Q79 282\r\nAAQQA 169\r\nA4K3T 968\r\n55559 703\r\nK853J 801\r\n2752A 448\r\n67779 75\r\n3QJ3T 649\r\n46T98 845\r\n8QTT2 92\r\nK5K53 608\r\nTJJ5K 959\r\n8KJ2T 946\r\n87828 135\r\nKKK5K 903\r\n4AJQ4 847\r\nKK7K3 560\r\n44A4Q 941\r\n4A845 580\r\n6Q4QQ 498\r\nA7JAJ 273\r\nK9K9K 167\r\nTTQTT 478\r\n85865 367\r\nTJJ4K 370\r\n63JAA 964\r\n2KA27 161\r\nQJ3KQ 232\r\n2Q332 865\r\nK5855 233\r\nQ66J9 871\r\n9496K 709\r\n22424 485\r\n78KAK 176\r\nKQ26T 189\r\n9JK8T 223\r\n898K3 474\r\n77J8J 726\r\nA6AKA 624\r\nTQ3J6 155\r\n86K8K 910\r\n4Q9T2 970\r\n999KK 640\r\n45JAK 327\r\n9AQT7 911\r\nQ6272 463\r\n99639 40\r\n7T8KA 648\r\nT5K7J 866\r\nA8JAA 952\r\n222KK 281\r\n39598 288\r\n994A4 721\r\nA38JA 423\r\n36AQK 892\r\n49576 72\r\nK999Q 287\r\nT77AA 885\r\nJ6J95 928\r\n65596 354\r\n2KA79 374\r\n6333J 861\r\n6553T 412\r\n3674K 446\r\nT46J6 717\r\n6Q26Q 909\r\nJJ444 556\r\n22922 333\r\nK3582 62\r\nTA5K2 674\r\nTQK48 651\r\n6A66A 610\r\nJ4664 251\r\nJ7Q87 848\r\nKJJ59 314\r\nQ33JQ 213\r\nQ4QQ4 611\r\nJ82J5 402\r\n9A889 588\r\n7J26K 34\r\n86T82 187\r\nAA5A2 353\r\n3J5T4 98\r\nA9A99 873\r\n99434 292\r\n9T8T9 822\r\n66J66 602\r\nJ5757 382\r\nT5578 378\r\n793A3 883\r\nT9989 682\r\nAKK34 234\r\n82695 654\r\nJAAJA 272\r\nTKT3T 326\r\n2KQ87 325\r\n955K5 661\r\n7AAA7 212\r\n336K3 384\r\n6J696 122\r\nAK2K8 791\r\nQQA46 296\r\nT7354 199\r\n8T894 359\r\n334J4 236\r\nQ6Q6Q 388\r\nJ9923 64\r\n2J296 496\r\nQ5JQQ 151\r\n6324A 126\r\n344T4 953\r\nKQT25 711\r\nJ557J 657\r\nKKK54 310\r\nK777T 937\r\n8K8K2 139\r\n5J969 856\r\n74974 539\r\nTT696 948\r\n5KQ36 769\r\nAAA72 295\r\n6J39J 279\r\n8868K 132\r\n779J9 79\r\n98Q59 973\r\nQAAA9 558\r\n2222J 494\r\n55757 255\r\nJ5T89 265\r\n25AA2 993\r\n63668 582\r\nTQ446 364\r\n44K8J 247\r\n53TA3 144\r\nK5J66 116\r\n85888 676\r\nJJ849 426\r\n293KQ 566\r\n3QQKQ 979\r\nTA83A 920\r\n22J72 795\r\n92527 171\r\n24Q46 766\r\nQ4777 731\r\n393TJ 820\r\n234J3 965\r\nKKJ52 829\r\n8J979 152\r\nJ6Q4Q 673\r\n7QJAA 670\r\n7K766 307\r\n9QT86 207\r\n82K88 555\r\nKQKKK 406\r\n664T4 115\r\nA8K34 487\r\n33K33 202\r\n6TK37 271\r\n42744 544\r\n662TT 420\r\n3KK33 590\r\n55589 963\r\nTK779 767\r\n7Q84K 304\r\n37A3A 619\r\n37338 414\r\n88JA8 25\r\nJ74Q8 572\r\nQJQ3Q 664\r\n5255J 322\r\n9K6J8 918\r\n576Q6 730\r\n444J3 19\r\n8K663 804\r\n7J7J4 3\r\n4A424 284\r\nA2AAA 811\r\nJ553J 405\r\n63KJA 950\r\nJTT89 554\r\nA5TAT 609\r\n77477 519\r\nTJT82 422\r\n77472 302\r\n85T2K 87\r\nA87A5 27\r\n37864 739\r\nKAAAK 660\r\n2T2T2 321\r\n34T43 390\r\n7JK48 449\r\nK4835 387\r\n26622 410\r\n33377 783\r\n84544 105\r\n99588 708\r\nA3T3A 686\r\n7K6Q3 124\r\n33939 512\r\nKK9KK 839\r\n6892Q 351\r\nTT9TT 335\r\n975JA 587\r\nJ2449 392\r\n8888J 436\r\n3359A 365\r\n695A9 36\r\n93J8A 526\r\n4T67A 146\r\n2222Q 825\r\nQ7Q2J 401\r\n45447 300\r\n384A3 77\r\nA855J 101\r\n2T8T6 430\r\nJJ6Q2 250\r\nJK22K 332\r\n4K226 491\r\n88QKQ 51\r\nK9KJA 919\r\n9995K 925\r\n24442 913\r\n5KKJ5 710\r\n4T3AA 723\r\nTTT4T 316\r\nJKQ9K 12\r\n44T22 277\r\n876T8 469\r\n66776 358\r\n33382 691\r\nAKKAJ 930\r\n88Q8Q 114\r\n6JQ6Q 385\r\n299J9 141\r\n3AJ9Q 812\r\n2AA72 43\r\n9KKJK 22\r\nQ2Q25 153\r\n5855J 589\r\nK4JK5 372\r\nAKAAA 347\r\nK72J7 754\r\nJTAAA 356\r\nT9AJ2 48\r\nK3348 779\r\nQK3J5 444\r\n63J97 29\r\nA4AQA 688\r\nJ3387 853\r\nA9J73 160\r\n54772 914\r\n52325 149\r\nJA2TA 576\r\n774AA 752\r\n544AA 823\r\n52J22 951\r\n93984 536\r\nK5J77 592\r\n7J7J7 481\r\n6328Q 716\r\n7T77A 761\r\nA8QQ9 174\r\nAA8AA 218\r\n89A9A 629\r\n2T82T 13\r\nK6KKK 985\r\n3Q34Q 380\r\n4798T 803\r\n34444 84\r\nT7AQ8 899\r\nT32Q3 276\r\n66363 477\r\nJ2992 942\r\nJ4555 904\r\n4JTAJ 598\r\n5J955 227\r\n46446 765\r\n79773 123\r\n2AQQQ 118\r\n333J3 827\r\nATA7T 162\r\n8T882 159\r\n66J6J 571\r\nA88QQ 363\r\nTT999 581\r\n66K6K 224\r\n33AAA 471\r\nKKAAK 638\r\nQJJA9 55\r\n22J2K 656\r\nTJQTT 760\r\nQJ76T 641\r\nT7J9T 615\r\nK4KKK 198\r\nA858J 978\r\n5J555 567\r\n888T3 834\r\nTQQTQ 647\r\n37KJ9 184\r\n57J6A 530\r\n6272A 376\r\nA5AAT 831\r\nKK8K8 879\r\n5665J 854\r\n28665 32\r\n2J6KA 532\r\n55893 816\r\nTT8T6 346\r\n995J9 379\r\n5KQKQ 958\r\n444A7 24\r\nQ5T26 188\r\n7589T 66\r\n9AJAA 552\r\n44KA6 254\r\n8K27T 91\r\n4Q954 28\r\n75QA8 129\r\n55565 545\r\nQ99Q9 922\r\n43999 836\r\n3J33A 54\r\n6J69J 729\r\n27A2J 881\r\nA3854 194\r\n52252 68\r\nJ22J2 204\r\n8J778 231\r\nKA8K3 301\r\n22329 434\r\nQ8K3Q 16\r\n88Q8J 74\r\n55TK5 360\r\n6A424 210\r\n88749 125\r\nKQTQT 685\r\n25A5K 635\r\n58KK8 14\r\n2TTTT 510\r\nK7777 303\r\nK5827 235\r\nK565K 819\r\n99969 266\r\n9J276 999\r\nT6K56 104\r\n44K3J 328\r\nT3T32 39\r\n3323Q 103\r\nKKKTT 563\r\nTAT6T 535\r\nT4T44 350\r\n78467 821\r\n66665 578\r\nTT2T2 898\r\n88KA6 867\r\n333Q7 71\r\n8J88J 932\r\n66K66 551\r\n5577T 927\r\nJJ999 447\r\nK3KJJ 313\r\n9T863 90\r\nQT333 466\r\nJ799T 650\r\nJTQ34 381\r\n38A33 690\r\n6JT36 712\r\n845A6 506\r\nKKJTK 259\r\n5KK6J 206\r\n4T69J 86\r\nJ2625 311\r\n979J9 818\r\nT9J67 419\r\nJ2JQQ 599\r\n52K5K 244\r\n8KQ4Q 462\r\n666Q3 893\r\nQ997Q 997\r\n59999 408\r\nKQA88 618\r\nJ35T8 158\r\n6AJ66 6\r\n57378 156\r\nQT24T 586\r\n4T546 850\r\n79TA2 237\r\nA7585 793\r\n9TT9Q 138\r\nK4K4J 154\r\nK863K 345\r\nJ4675 887\r\n86886 2\r\n455A6 658\r\nA5QQ5 341\r\n69J9J 165\r\nKT592 178\r\nTKTKT 944\r\n3JTT5 774\r\nJ5Q24 404\r\n49J99 415\r\n2Q5AQ 943\r\n22226 109\r\n95J9T 593\r\n4K44K 524\r\n428J4 428\r\nA6JA6 21\r\nTT7J7 601\r\n96TJQ 735\r\n4K4TT 719\r\nJAAAA 983\r\nK397A 357\r\nJA348 200\r\nK8888 732\r\nJ934K 488\r\n76J37 201\r\nQJTT2 628\r\n356KA 442\r\n99JKK 547\r\nK7J73 934\r\nQ2Q2T 955\r\nKK76K 782\r\n66685 912\r\n34888 140\r\n33533 809\r\nK7KKK 637\r\n67777 665\r\n7KJKK 468\r\nTTT64 789\r\n57QK9 137\r\n52TJ9 197\r\n6JA4A 626\r\nTTT4A 458\r\n8A488 663\r\n445Q4 513\r\n99933 136\r\n95J89 780\r\n5J55J 787\r\nT33T3 606\r\nQJ348 725\r\nT8T88 753\r\n89T98 248\r\nK6J7K 164\r\nT66A9 59\r\nJJQQ3 502\r\n44433 456\r\n49254 425\r\nKK7K7 810\r\n8KAQQ 186\r\n6AQQ9 185\r\nKQ477 495\r\nT77TT 698\r\n68JT9 798\r\n6AJJ2 343\r\n4J448 949\r\n39636 623\r\n4KK8T 901\r\nQ8KQ7 441\r\n39333 221\r\nAAAA9 672\r\n53535 768\r\n77446 294\r\n6A37T 639\r\n32333 508\r\nJJ994 286\r\nA77KA 705\r\nQQQQ3 9\r\nA88KA 17\r\n6JKT3 418\r\n3996J 864\r\nAAJ67 625\r\n2JJ2Q 718\r\n444KQ 908\r\nA8J74 860\r\nT3J3A 906\r\n6J873 134\r\n7Q58Q 192\r\n994A2 851\r\nAA99A 50\r\n4J2K5 355\r\nATTTA 504\r\nQTTQT 764\r\n85A85 744\r\nJK378 736\r\n828AJ 799\r\n47247 411\r\n78T8T 226\r\nTK888 756\r\n626T2 315\r\n226J6 438\r\n5Q9QQ 290\r\n88886 742\r\nA5TK8 974\r\nKK36K 330\r\n99J96 562\r\n72K72 203\r\nTJK79 857\r\n6AAA4 52\r\nQ3K7T 813\r\n5AJ6Q 938\r\n7JQ66 398\r\n44QQ2 128\r\n8J833 802\r\n46K2J 894\r\n9559J 317\r\nAAKK2 671\r\nJ8688 669\r\n9TTK6 44\r\nJ5655 439\r\n3K865 747\r\n36AT3 777\r\nA4A4A 480\r\n32JQQ 936\r\nK8T98 291\r\nTQ752 119\r\nJ5924 727\r\nQQ7QQ 643\r\n7Q7Q6 83\r\n2JJ99 267\r\nAATJT 497\r\nKQ6Q6 683\r\n355JQ 878\r\nK632Q 242\r\nJQ792 457\r\nA555A 58\r\n255JJ 53\r\n63356 274\r\n66ATT 520\r\n7AK77 622\r\n975A8 607\r\n63333 833\r\nTTTT8 981\r\nKTTKQ 18\r\nJTTJT 655\r\nA34AA 453\r\n5QKJ6 888\r\n85833 874\r\nJJ33T 531\r\nAT222 260\r\n5A5AT 435\r\n388A3 994\r\n32323 299\r\nK4394 781\r\nQA3AA 773\r\n559KK 111\r\n88388 257\r\n87727 947\r\n9JKA9 416\r\n45T8J 697\r\n3AA88 828\r\nJJ43T 131\r\n9K29T 763\r\n6J444 121\r\n9999J 977\r\n3QTA2 743\r\n78439 905\r\n49653 826\r\nAAJAQ 108\r\n8QJ46 967\r\n4TQA8 484\r\n5933J 7\r\nQ6Q5Q 844\r\n4A4J4 960\r\n72TT4 741\r\n45343 240\r\nAAAQ7 684\r\nJ2777 8\r\n52226 323\r\nTA472 38\r\n33J3J 81\r\nA5A36 100\r\nA52J2 76\r\n5J7Q5 645\r\n99TTT 293\r\n44AA3 855\r\n89JK8 603\r\n48884 569\r\n77887 80\r\nK5555 614\r\n4T29J 37\r\n4T2T4 289\r\nQQ55Q 923\r\nJ564J 876\r\nJQ6KA 437\r\nAA848 112\r\n5A5J3 35\r\n4976J 916\r\n39Q75 264\r\n4K555 842\r\nAA6AA 921\r\nQT2QT 377\r\n855Q8 85\r\n844Q2 694\r\n35555 702\r\n456Q6 258\r\nJ2326 262\r\n4Q6A2 849\r\nAA7AA 882\r\nJA94A 679\r\n48888 538\r\nQ7777 786\r\nQ288T 890\r\n5J8Q8 517\r\n84433 49\r\n9889J 612\r\n96969 246\r\n7JTAA 557\r\n3T6J5 668\r\n9KA29 57\r\nA333A 298\r\n2QJQQ 835\r\n9K933 543\r\nJ27AK 525\r\n2J525 366\r\n999J3 352\r\n67A69 840\r\n4888K 56\r\n9TJ6T 577\r\n36886 173\r\n5445K 516\r\nA829A 486\r\nTJTTT 740\r\n4JQ94 493\r\n479J3 681\r\nKAA3A 96\r\n77733 808\r\nQ3Q3T 507\r\nT888J 595\r\n7KQ22 751\r\nQQTQQ 399\r\n69666 31\r\n65676 636\r\n87988 503\r\nKJT9K 917\r\nJTA83 433\r\n2622Q 417\r\n2QA75 489\r\n779QA 371\r\nJJJKK 749\r\n43327 470\r\n936A8 476\r\n3KAQ9 163\r\nTQK6K 89\r\n64444 772\r\n94T63 815\r\nAA8A8 249\r\n2Q9A4 895\r\n78QJ3 452\r\nAA5A5 992\r\nT9TT5 565\r\nK39Q7 935\r\nJ9TJ9 662\r\n78KKJ 746\r\nA4954 395\r\n7K4K9 349\r\n57KJ3 467\r\nAQA8Q 984\r\n7A234 523\r\nT7777 443\r\n626K3 701\r\n7778A 179\r\n55885 391\r\n7249A 239\r\n55Q56 229\r\n43772 117\r\nJQTQT 986\r\n74J44 915\r\nTTT7T 431\r\nKAATJ 807\r\nT5T55 518\r\nQJ2J8 329\r\n8K8K8 166\r\nAJA99 450\r\nQAJ58 817\r\n69226 737\r\nK2KKK 677\r\n78868 142\r\n2QTTT 597\r\n9A999 696\r\nQQQAQ 666\r\nAAT2A 63\r\n7J777 93\r\n2QQQQ 956\r\n33423 4\r\nJ3862 987\r\nA7KKK 642\r\n44777 541\r\nT9647 46\r\n6T929 465\r\n2TTT6 245\r\nJ7J7J 95\r\nT66KK 931\r\n689TK 182\r\nT3558 771\r\n6T4A2 594\r\n87887 998\r\nTT445 564\r\nT5QQJ 695\r\n27J4K 969\r\nJQQJQ 715\r\n59873 180\r\nK252K 858\r\n364Q4 806\r\nA972J 427\r\n32372 61\r\nAJ939 775\r\nJQQ7Q 907\r\n95A55 263\r\n88282 193\r\nTJQ2J 896\r\n338J9 838\r\nK4K9A 490\r\n444J4 796\r\nT3A9Q 147\r\n23992 634\r\n3J98T 515\r\n7K477 680\r\n9T7A3 527\r\n8QT88 130\r\nKJ68J 500\r\n5888J 70\r\n384K2 884\r\n76TTT 790\r\n27AA7 800\r\nQQQAA 591\r\n6333A 846\r\nQQ373 926\r\n32232 172\r\nA566J 692\r\n2J4A3 794\r\nTTAQQ 511\r\n584J8 88\r\n6KK76 383\r\nA7AAK 553\r\n33J35 429\r\n5AAAA 631\r\n3AA2K 724\r\nT9Q78 220\r\nTT4TK 113\r\n7553A 604\r\nQQQ9Q 713\r\nQ7Q97 980\r\n4K6KK 344\r\n2J877 788\r\nAQ33J 1000\r\n3888K 870\r\nJA55A 445\r\n66622 268\r\nT5JKK 616\r\n77T56 521\r\nKK8A8 461\r\nK34K4 733\r\n6KJQQ 797\r\nKA329 600\r\nAJAA7 324\r\n99Q8A 451\r\nJ7662 252\r\n933J9 830\r\n97797 373\r\nAAA66 473\r\n24TQ8 243\r\n96Q4Q 633\r\nTAAA3 483\r\nQT2A4 841\r\n29929 966\r\n675KT 785\r\n2JJJ5 280\r\n7Q7J6 253\r\n88444 389\r\nQ6996 762\r\n69753 514\r\n7J9AT 722\r\n86866 534";

        private static IEnumerable<Hand> ParseHands(string input) =>
            input.Split(Environment.NewLine)
                .Select(x => x.Split(" "))
                .Select(x => new Hand
                {
                    Cards = x[0].ToUpper(),
                    Wager = int.Parse(x[1])
                });

        private static HandType TypeWithoutJokers(IEnumerable<(char, int)> groupedCards) =>
            groupedCards.Select(x => x.Item2).ToArray() switch
            {
                [5] => HandType.FiveOfKind,
                [4, .._] => HandType.FourOfKind,
                [3, 2] => HandType.FullHouse,
                [3, .._] => HandType.ThreeOfKind,
                [2, 2, .._] => HandType.TwoPair,
                [2, .._] => HandType.OnePair,
                _ => HandType.HighestCard,
            };

        private static HandType TypeWithJokers(IEnumerable<(char, int)> groupedCards, int numJokers) =>
            (numJokers, Score: TypeWithoutJokers(groupedCards)) switch
            {
                (1, HandType.HighestCard) => HandType.OnePair,
                ( 1, HandType.OnePair ) => HandType.ThreeOfKind,
                (1, HandType.TwoPair) => HandType.FullHouse,
                (1, HandType.ThreeOfKind) => HandType.FourOfKind,
                (1, HandType.FourOfKind) => HandType.FiveOfKind,
                (2, HandType.HighestCard) => HandType.ThreeOfKind,
                (2, HandType.OnePair) => HandType.FourOfKind,
                (2, HandType.TwoPair) => HandType.FourOfKind,
                (2, HandType.ThreeOfKind) => HandType.FiveOfKind,
                (3, HandType.HighestCard) => HandType.FourOfKind,
                (3, HandType.OnePair) => HandType.FiveOfKind,
                (4, _) => HandType.FiveOfKind,
                (5, _) => HandType.FiveOfKind
            };

        private static HandType TypeWithJokers(IEnumerable<(char, int)> groupedCards)
        {
            var groupedCardsArray = groupedCards as (char, int)[] ?? groupedCards.ToArray();
            return groupedCardsArray.Any(x => x.Item1 == 'J')
                ? TypeWithJokers(groupedCardsArray.Where(x => x.Item1 != 'J'), groupedCardsArray.Sum(x => x.Item1 == 'J' ? x.Item2 : 0))
                : TypeWithoutJokers(groupedCardsArray);
        }

        private static HandWithScoreType DetermineScoreType(Hand h, bool jokers = false)
        {
            var groupedCards = h.Cards.GroupBy(x => x)
                .Select(x => (x.Key, x.Count()))
                .OrderByDescending(x => x.Item2)
                .ToArray();

            var type = jokers
                ? TypeWithJokers(groupedCards)
                : TypeWithoutJokers(groupedCards);
            
            return new HandWithScoreType
            {
                Cards = h.Cards,
                Wager = h.Wager,
                Type = type
            };
        }

        private static IEnumerable<HandWithScoreTypeAndRank> RankHands(IEnumerable<HandWithScoreType> hands) =>
            hands.OrderBy(x => x.Type, HandTypeComparer)
                .ThenBy(x => x, handCardValueComparer)
                .Select((x, i) => new HandWithScoreTypeAndRank
                {
                    Cards = x.Cards,
                    Type = x.Type,
                    Wager = x.Wager,
                    Rank = i + 1
                });


        [Fact]
        public void Day07_Test01()
        {
            handCardValueComparer = new HandCardValueComparer();
            const string input = "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483";
            var hands = ParseHands(input);
            hands.Should().BeEquivalentTo(new[]
            {
                new Hand {Cards = "32T3K", Wager = 765},
                new Hand {Cards = "T55J5", Wager = 684},
                new Hand {Cards = "KK677", Wager = 28},
                new Hand {Cards = "KTJJT", Wager = 220},
                new Hand {Cards = "QQQJA", Wager = 483}
            });
        }

        [Fact]
        public void Day07_Test02()
        {
            handCardValueComparer = new HandCardValueComparer();
            const string input = "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483";
            var hands = ParseHands(input);
            var handTypes = hands.Select(x => DetermineScoreType(x)).ToArray();
            handTypes.Should().BeEquivalentTo(new[]
            {
                new HandWithScoreType {Cards = "32T3K", Wager = 765, Type = HandType.OnePair },
                new HandWithScoreType {Cards = "T55J5", Wager = 684, Type = HandType.ThreeOfKind },
                new HandWithScoreType {Cards = "KK677", Wager = 28, Type  = HandType.TwoPair },
                new HandWithScoreType {Cards = "KTJJT", Wager = 220, Type  = HandType.TwoPair },
                new HandWithScoreType {Cards = "QQQJA", Wager = 483, Type = HandType.ThreeOfKind }
            });
        }

        [Fact]
        public void Day07_Test03()
        {
            handCardValueComparer = new HandCardValueComparer();
            const string input = "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483";
            var hands = ParseHands(input);
            var handTypes = hands.Select(x => DetermineScoreType(x)).ToArray();
            var rankedHands = RankHands(handTypes);
            rankedHands.Should().BeEquivalentTo(new[]
            {
                new HandWithScoreTypeAndRank {Cards = "32T3K", Wager = 765, Type = HandType.OnePair, Rank = 1},
                new HandWithScoreTypeAndRank {Cards = "T55J5", Wager = 684, Type = HandType.ThreeOfKind, Rank = 4 },
                new HandWithScoreTypeAndRank {Cards = "KK677", Wager = 28, Type  = HandType.TwoPair, Rank = 3 },
                new HandWithScoreTypeAndRank {Cards = "KTJJT", Wager = 220, Type  = HandType.TwoPair, Rank = 2 },
                new HandWithScoreTypeAndRank {Cards = "QQQJA", Wager = 483, Type = HandType.ThreeOfKind, Rank = 5 }
            });
        }

        [Fact]
        public void Day07_Test04()
        {
            handCardValueComparer = new HandCardValueComparer();
            const string input = "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483";
            var hands = ParseHands(input);
            var handTypes = hands.Select(x => DetermineScoreType(x)).ToArray();
            var rankedHands = RankHands(handTypes);
            var finalScore = rankedHands.Sum(x => x.Rank * x.Wager);
            finalScore.Should().Be(6440);
        }

        [Fact]
        public void Day07_Part01()
        {
            handCardValueComparer = new HandCardValueComparer();
            var hands = ParseHands(RealInput);
            var handTypes = hands.Select(x => DetermineScoreType(x)).ToArray();
            var rankedHands = RankHands(handTypes);
            var finalScore = rankedHands.Sum(x => x.Rank * x.Wager);
            finalScore.Should().Be(248569531);
        }

        [Fact]
        public void Day07_Test05()
        {
            handCardValueComparer = new HandCardValueComparer(true);
            const string input = "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483";
            var hands = ParseHands(input);
            var handTypes = hands.Select(x => DetermineScoreType(x, true)).ToArray();
            var rankedHands = RankHands(handTypes);
            var finalScore = rankedHands.Sum(x => x.Rank * x.Wager);
            finalScore.Should().Be(5905);
        }

        [Fact]
        public void Day07_Part02()
        {
            handCardValueComparer = new HandCardValueComparer(true);
            var hands = ParseHands(RealInput);
            var handTypes = hands.Select(x => DetermineScoreType(x, true)).ToArray();
            var rankedHands = RankHands(handTypes);
            var finalScore = rankedHands.Sum(x => x.Rank * x.Wager);
            finalScore.Should().Be(250382098);
        }


    }
}
