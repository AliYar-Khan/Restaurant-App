import { OpenTelemetry } from './instrumentation';
const otel = OpenTelemetry('checkout-api')
otel.start();

import router from './routes/routes';
import { logger } from './utils/logger';
import { config } from './config/config';
import express from 'express';
import pinoHTTP from 'pino-http';
import checkoutPublisher from './messagging/publisher';


async function main() {
  try {
    await checkoutPublisher.start();
  } catch (error) {
    logger.error("kafka producer connect failed:", error);
    process.exit(1);
  }
  const app = express();
  app.use(express.json());
  app.use(
    pinoHTTP({
      logger,
    })
  );

  app.use(config.baseUrl, router)

  app.listen(Number(config.port), config.host, () => {
    logger.info(`⚡️[server]: Server is running at http://${config.host}:${config.port}`);
  });

  process.on('SIGINT', async () => {
    await otel.shutdown();
    await checkoutPublisher.shutdown();
    logger.info("Gracefully shutting down from SIGINT (Ctrl-C)");
    // some other closing procedures go here
    process.exit(0);
  });
}

main();


