import { HardhatUserConfig } from "hardhat/config";
import "@nomicfoundation/hardhat-toolbox";
import * as dotenv from "dotenv";

dotenv.config();

const config = {
  solidity: "0.8.19", // solidity version
  defaultNetwork: "mantleTest", // chosen by default when network isn't specified while running Hardhat
  networks: {
    mantle: {
      url: "https://rpc.mantle.xyz", // Mainnet
      accounts: [process.env.ACCOUNT_PRIVATE_KEY ?? ""],
    },
    mantleTest: {
      url: "https://rpc.testnet.mantle.xyz", // Testnet
      accounts: [process.env.ACCOUNT_PRIVATE_KEY ?? ""],
    },
    mantleCustom: {
      url: "https://custom-rpc.mantle.xyz", // Custom network (replace with your custom RPC URL)
      accounts: [process.env.ACCOUNT_PRIVATE_KEY ?? ""],
    },
    zircuit: {
      url: `https://zircuit1.p2pify.com`,
      accounts: [process.env.ZIRCUIT_PRIVATE_KEY],
    },
  },
  etherscan: {
    apiKey: process.env.API_KEY,
  },
};
